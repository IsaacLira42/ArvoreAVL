# Lógica de Visualização da Árvore

Este arquivo detalha o funcionamento do código responsável por desenhar a árvore AVL no console em um formato de cima para baixo.

## Lógica de Funcionamento (Explicação Conceitual)

O principal desafio ao imprimir uma árvore no console é representar uma estrutura bidimensional (que tem largura e altura) em um ambiente linear (uma sequência de linhas de texto). Para que os nós apareçam alinhados corretamente acima de seus filhos, precisamos de uma estratégia de duas etapas:

### 1. Coleta de Nós em Nível (Busca em Largura)

Primeiro, precisamos saber quais nós existem em cada nível da árvore. A maneira mais eficaz de fazer isso é com um algoritmo de "Busca em Largura" (ou Level-Order Traversal), que funciona da seguinte forma:

-   **Uso de uma Fila:** Utilizamos uma fila (uma estrutura de "primeiro a entrar, primeiro a sair") para rastrear os nós que precisamos visitar.
-   **Início:** Começamos colocando o nó raiz na fila.
-   **Processamento em Loop:** Enquanto a fila não estiver vazia, repetimos um processo para formar cada nível da árvore:
    -   Determinamos quantos nós estão na fila naquele momento. Esse número corresponde à quantidade de nós no nível atual.
    -   Criamos uma lista para armazenar os nós desse nível.
    -   Retiramos cada um desses nós da fila, um por um, e os adicionamos à nossa lista de nível.
    -   **O Ponto Chave:** Para cada nó que retiramos, adicionamos seus dois filhos (esquerdo e direito) ao final da fila. Se um filho não existir (for nulo), adicionamos um `null` na fila. Isso é crucial, pois esses `null`s atuam como "placeholders" (marcadores de posição), garantindo que a estrutura da árvore seja preservada e que o espaçamento no momento da impressão seja correto.
-   **Critério de Parada:** Armazenamos la lista de cada nível em uma lista principal. O processo para quando processamos um nível que contém apenas `null`s, o que significa que chegamos ao fim da árvore.

Ao final desta etapa, temos uma estrutura de dados (uma lista de listas) que representa a árvore perfeitamente, nível por nível, incluindo os espaços vazios.

### 2. Impressão Formatada no Console

Com a lista de níveis em mãos, a impressão se torna um problema de formatação:

-   **Iteração por Nível:** Percorremos a lista principal, imprimindo um nível de cada vez.
-   **Cálculo de Espaçamento:** Para cada nível, dividimos a largura total do console em "slots" (fatias ou células) iguais. O número de slots é igual ao número de itens na lista daquele nível (incluindo os `null`s).
-   **Centralização:** Para cada nó, imprimimos seu valor de forma centralizada dentro do seu slot. Se o item for um `null` (um placeholder), simplesmente imprimimos espaços em branco.

Essa técnica garante que um nó no `nível X` apareça visualmente centralizado acima de onde seus filhos estarão no `nível X+1`, criando um layout de árvore claro e legível no console.

### Exemplo Prático Passo a Passo

Vamos visualizar o processo com uma árvore simples. Imagine que inserimos os números `20`, `10`, `30` e `5`. A árvore resultante seria:

```
      20
     /  \
    10   30
   /
  5
```

#### Etapa 1: Coleta de Nós

O algoritmo `GetNodesByLevel` irá percorrer a árvore:

1.  **Início:**
    -   `fila` (Queue) contém: `[20]`
    -   `níveis` (List of lists) está: `[]`

2.  **Processando Nível 0:**
    -   O nó `20` é retirado da fila. A lista do nível atual é `[20]`.
    -   Os filhos de `20` (`10` e `30`) são adicionados à fila.
    -   **Resultado:**
        -   `fila`: `[10, 30]`
        -   `níveis`: `[[20]]`

3.  **Processando Nível 1:**
    -   Os nós `10` e `30` são retirados da fila. A lista do nível atual é `[10, 30]`.
    -   Filhos de `10` (`5` e `null`) são adicionados à fila.
    -   Filhos de `30` (`null` e `null`) são adicionados à fila.
    -   **Resultado:**
        -   `fila`: `[5, null, null, null]`
        -   `níveis`: `[[20], [10, 30]]`

4.  **Processando Nível 2:**
    -   Os itens `5, null, null, null` são retirados da fila. A lista do nível é `[5, null, null, null]`.
    -   Para cada item, seus filhos (que são todos `null`) são adicionados à fila.
    -   **Resultado:**
        -   `fila`: `[null, null, null, null, null, null, null, null]`
        -   `níveis`: `[[20], [10, 30], [5, null, null, null]]`

5.  **Processando Nível 3:**
    -   A fila contém 8 `null`s. O algoritmo processa este nível, mas como não há nenhum nó real (`hasNonNullNode` será `false`), o loop é interrompido.

**Resultado Final da Coleta:** A estrutura de dados `níveis` é: `[[20], [10, 30], [5, null, null, null]]`

#### Etapa 2: Impressão Formatada

Agora, o método `Print` itera sobre a lista `níveis` (assumindo uma largura de console de 80 caracteres):

1.  **Imprimindo Nível 0:** `[20]`
    -   Há 1 item, então cada "slot" tem 80 de largura.
    -   Saída: `20` (centralizado em 80 espaços)

2.  **Imprimindo Nível 1:** `[10, 30]`
    -   Há 2 itens, então cada "slot" tem 40 de largura.
    -   Saída: `10` (centralizado nos primeiros 40 espaços) e `30` (centralizado nos 40 espaços seguintes).

3.  **Imprimindo Nível 2:** `[5, null, null, null]`
    -   Há 4 itens, então cada "slot" tem 20 de largura.
    -   Saída: `5` (no primeiro slot), seguido por três slots vazios.

**Visualização Final no Console:**

(A aparência exata depende dos fatores de balanceamento, mas a estrutura será esta)

```
                                     20[1]

                 10[1]                                               30[0]

      5[0]
```

## Código Comentado

Abaixo está o código C# que implementa a lógica descrita. Ele é encapsulado em uma classe estática `TreePrinter` para separar a responsabilidade de impressão da lógica principal da árvore AVL.

```csharp
// NOTA: Este código depende da classe Node<T> definida no seu projeto.
// A definição é semelhante a esta:
/*
public class Node<T>
{
    public T Dado { get; set; }
    public Node<T>? Esquerda { get; set; }
    public Node<T>? Direita { get; set; }
    public Node<T>? Pai { get; set; }
    public int FatorBalanceamento { get; set; }
    // ... construtor ...
}
*/

using System;
using System.Collections.Generic;

// Classe estática para não precisar ser instanciada.
// Contém toda a lógica para imprimir a árvore.
public static class TreePrinter
{
    /// <summary>
    /// Método principal que imprime a árvore no console.
    /// </summary>
    /// <param name="root">O nó raiz da árvore a ser impressa.</param>
    public static void Print<T>(Node<T>? root) where T : IComparable<T>
    {
        if (root == null)
        {
            Console.WriteLine("Árvore vazia");
            return;
        }

        // Etapa 1: Coletar todos os nós, organizados por nível.
        var levels = GetNodesByLevel(root);
        int consoleWidth = 80; // Largura da área de impressão.

        // Etapa 2: Iterar sobre cada nível e imprimi-lo com a formatação correta.
        foreach (var level in levels)
        {
            // Calcula a largura de cada "slot" para os nós deste nível.
            int slotCount = level.Count;
            int slotWidth = consoleWidth / slotCount;
            string line = "";

            foreach (var node in level)
            {
                string nodeText = "";
                if (node != null)
                {
                    // Formata o texto do nó para incluir o fator de balanceamento.
                    nodeText = $"{node.Dado}[{node.FatorBalanceamento}]";
                }

                // Calcula o preenchimento (padding) para centralizar o texto no slot.
                int padding = (slotWidth - nodeText.Length) / 2;
                if (padding < 0) padding = 0;

                // Monta a linha de texto para o nível atual.
                line += new string(' ', padding);
                line += nodeText;
                line += new string(' ', slotWidth - padding - nodeText.Length);
            }
            Console.WriteLine(line);
            Console.WriteLine(); // Adiciona uma linha em branco para espaçamento vertical.
        }
    }

    /// <summary>
    /// Coleta os nós da árvore e os organiza em uma lista de listas, onde cada lista interna representa um nível.
    /// </summary>
    /// <param name="root">O nó raiz da árvore.</param>
    /// <returns>Uma lista de listas contendo os nós de cada nível.</returns>
    private static List<List<Node<T>?>> GetNodesByLevel<T>(Node<T> root) where T : IComparable<T>
    {
        var levels = new List<List<Node<T>?>>();
        var queue = new Queue<Node<T>?>();
        queue.Enqueue(root);

        // Continua o loop enquanto houver nós na fila para processar.
        while (queue.Count > 0)
        {
            var currentLevel = new List<Node<T>?>();
            int levelSize = queue.Count;
            bool hasNonNullNode = false; // Flag para verificar se o nível contém algum nó real.

            // Processa todos os nós do nível atual.
            for (int i = 0; i < levelSize; i++)
            {
                var node = queue.Dequeue();
                currentLevel.Add(node);

                if (node != null)
                {
                    // Se o nó não for nulo, marca a flag e enfileira seus filhos.
                    hasNonNullNode = true;
                    queue.Enqueue(node.Esquerda);
                    queue.Enqueue(node.Direita);
                }
                else
                {
                    // Se o nó for nulo, enfileira dois placeholders nulos para manter a estrutura.
                    queue.Enqueue(null);
                    queue.Enqueue(null);
                }
            }

            // Só adiciona o nível à lista se ele contiver pelo menos um nó real.
            if (hasNonNullNode)
            {
                levels.Add(currentLevel);
            }
            else
            {
                // Se o nível inteiro for de nulos, chegamos ao fim da árvore.
                break;
            }
        }
        return levels;
    }
}
```
