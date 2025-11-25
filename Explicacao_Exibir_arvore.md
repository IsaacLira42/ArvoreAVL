# Guia de Estudo: Exibindo a Árvore AVL no Console

A capacidade de visualizar a estrutura de uma árvore de dados é uma ferramenta de depuração extremamente poderosa. A sua implementação faz isso de uma maneira inteligente, dividindo o trabalho em duas etapas principais, encapsuladas em dois métodos:

1.  `ObterNosPorNivel()`: Este método é o "fotógrafo". Ele inspeciona a árvore e captura sua estrutura, nível por nível, em uma lista de listas.
2.  `Mostrar()`: Este método é o "impressor". Ele pega os dados estruturados do método anterior e os formata para exibição no console, tentando centralizar os nós para uma melhor visualização.

Vamos analisar cada um em detalhes.

---\n

## 1. `ObterNosPorNivel()` - A "Fotografia" da Árvore

O objetivo deste método é converter a estrutura de nós interligados da árvore em uma `List<List<Node<T>?>>`, onde cada lista interna representa um nível completo da árvore.

Para fazer isso, ele utiliza um algoritmo de **Busca em Largura (Breadth-First Search - BFS)**, que é perfeito para processar uma árvore nível por nível. A grande sacada aqui é o uso de uma `Queue` (Fila) e o tratamento de nós `null`.

### O Código Comentado

```csharp
private List<List<Node<T>?>> ObterNosPorNivel()
{
    // A lista final que conterá todos os níveis.
    var niveis = new List<List<Node<T>?>>();
    if (Raiz == null) return niveis; // Se a árvore está vazia, retorna a lista vazia.

    // A fila é a base da busca em largura. Começamos com a raiz.
    var fila = new Queue<Node<T>?>();
    fila.Enqueue(Raiz);

    while (fila.Count > 0)
    {
        // Cada iteração do 'while' processa um nível completo.
        var nivelAtual = new List<Node<T>?>();
        int tamanhoNivel = fila.Count; // O número de nós (ou espaços nulos) neste nível.
        bool temNoNaoNulo = false; // Flag para otimização: parar quando um nível inteiro for nulo.

        // Loop para processar cada item do nível atual que está na fila.
        for (int i = 0; i < tamanhoNivel; i++)
        {
            // Pega o próximo nó da fila.
            var no = fila.Dequeue();
            nivelAtual.Add(no); // Adiciona na lista do nível atual.

            if (no != null)
            {
                // Se o nó não é nulo, ele pode ter filhos.
                temNoNaoNulo = true;
                // Enfileira os filhos (mesmo que sejam nulos!) para serem processados no próximo nível.
                fila.Enqueue(no.Esquerda);
                fila.Enqueue(no.Direita);
            }
            else
            {
                // Se o nó é nulo, seus "filhos" também são nulos.
                // Enfileiramos dois nulos para manter a estrutura e o espaçamento corretos.
                fila.Enqueue(null);
                fila.Enqueue(null);
            }
        }

        // Se o nível que acabamos de processar tinha pelo menos um nó real...
        if (temNoNaoNulo)
        {
            // ...adicionamos ele à nossa lista de níveis.
            niveis.Add(nivelAtual);
        }
        else
        {
            // Se o nível inteiro era de nulos, significa que chegamos ao fim da árvore.
            // Podemos parar e não continuar enfileirando nulos para sempre.
            break;
        }
    }
    return niveis;
}
```

**A Chave da Lógica Visual:** O passo mais importante é que, para **qualquer item** que tiramos da fila (seja um nó ou um `null`), nós **sempre** adicionamos seus dois "filhos" de volta na fila. É isso que preserva os "buracos" na árvore, permitindo que o método `Mostrar` saiba onde deve imprimir espaços em branco.

---\n

## 2. `Mostrar()` - Desenhando no Console

Com a estrutura da árvore perfeitamente organizada em uma lista de listas, o trabalho do método `Mostrar` é puramente cosmético: ele calcula espaçamentos para tentar desenhar algo que se pareça com uma árvore.

### O Código Comentado

```csharp
public void Mostrar()
{
    if (Raiz == null)
    {
        Console.WriteLine("Árvore vazia");
        return;
    }

    // 1. Pega a "fotografia" da árvore.
    var niveis = ObterNosPorNivel();
    int larguraConsole = 80; // Largura total para o desenho.

    // 2. Itera sobre cada nível da árvore.
    foreach (var nivel in niveis)
    {
        int quantidadeSlots = nivel.Count; // Quantos "lugares" temos neste nível.
        // 3. Calcula o espaço (slot) disponível para cada nó no nível atual.
        //    Níveis mais altos têm menos nós, então cada nó ganha mais espaço.
        int larguraSlot = larguraConsole / quantidadeSlots;
        string linha = ""; // A string que representará a linha do nível atual.

        // 4. Itera sobre cada nó (ou espaço nulo) no nível.
        foreach (var no in nivel)
        {
            string textoNo = "";
            if (no != null)
            {
                // Formata o texto que será exibido para o nó.
                textoNo = $"{no.Dado}[{no.FatorBalanceamento}]\n";
            }

            // 5. Calcula o preenchimento para centralizar o texto no seu "slot".
            int preenchimento = (larguraSlot - textoNo.Length) / 2;
            // Blindagem: se o texto for maior que o slot, o preenchimento pode ser negativo.
            if (preenchimento < 0) preenchimento = 0;
            
            // Outra blindagem para o espaço restante.
            int posPreenchimento = larguraSlot - preenchimento - textoNo.Length;
            if (posPreenchimento < 0) posPreenchimento = 0;

            // 6. Monta a string da linha: Espaços + Texto + Espaços.
            linha += new string(' ', preenchimento);
            linha += textoNo;
            linha += new string(' ', posPreenchimento);
        }
        // 7. Imprime a linha completa do nível e pula uma linha para espaçamento vertical.
        Console.WriteLine(linha);
        Console.WriteLine();
    }
}
```

**A Lógica do Espaçamento:**

A ideia é dividir a largura do console igualmente entre todos os "slots" de um nível.

-   No nível 0, há 1 slot (a raiz), então `larguraSlot` é 80. O texto da raiz será centralizado em 80 caracteres.
-   No nível 1, há 2 slots, então `larguraSlot` é 40. O filho esquerdo será centralizado nos primeiros 40 caracteres e o direito nos últimos 40.
-   No nível 2, há 4 slots, `larguraSlot` é 20. E assim por diante.

Isso cria um efeito de pirâmide que, embora não desenhe as "perninhas" (`/` e `\`) da árvore, dá uma excelente noção visual da sua forma e do seu balanceamento. As blindagens (`if (... < 0)`) foram as correções que fizemos para evitar que o programa quebrasse caso o texto do nó (`"dado[FB]"`) ficasse maior que o espaço disponível no slot.
