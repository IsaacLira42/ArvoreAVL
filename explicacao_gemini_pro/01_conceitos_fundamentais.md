# Guia de Estudo: Árvore AVL

Este guia foi gerado para te ajudar a estudar para a prova, usando a sua própria implementação como base.

## 1. O que é uma Árvore AVL?

Uma árvore AVL é uma **árvore binária de pesquisa (ABP) auto-balanceável**. O nome "auto-balanceável" é a característica principal.

Em uma ABP normal, o pior caso de complexidade para busca, inserção e remoção é **O(n)**, que ocorre quando a árvore se degenera em uma lista ligada (todos os nós inseridos em ordem crescente ou decrescente).

A árvore AVL resolve esse problema garantindo que a árvore permaneça "razoavelmente" balanceada o tempo todo. Ela faz isso mantendo a seguinte regra:

> **Regra de Ouro da AVL:** Para qualquer nó na árvore, as alturas de suas duas subárvores (esquerda e direita) podem diferir em no máximo 1.

Essa regra garante que a altura da árvore seja sempre **O(log n)**, o que torna as operações de busca, inserção e remoção muito eficientes, sempre com complexidade **O(log n)**.

## 2. O Fator de Balanceamento (FB)

Para garantir a regra de ouro, a AVL utiliza um conceito chamado **Fator de Balanceamento (FB)**. Ele é calculado para cada nó da árvore.

> **Fórmula:** `Fator de Balanceamento = Altura(Subárvore Esquerda) - Altura(Subárvore Direita)`

Na sua implementação, você armazena esse valor diretamente no nó, na propriedade `FatorBalanceamento`.

```csharp
// Em ArvoreAVL/Entities/Node.cs
public class Node<T>
{
    // ... outros campos
    public int FatorBalanceamento { get; set; }
    // ...
}
```

Com base no valor do FB, podemos saber o estado de um nó:

*   **FB = 0:** O nó está perfeitamente balanceado (as duas subárvores têm a mesma altura).
*   **FB = 1 (ou +1):** O nó está pendendo para a **esquerda** (subárvore esquerda é 1 nível mais alta que a direita). Isso ainda é considerado balanceado.
*   **FB = -1:** O nó está pendendo para a **direita** (subárvore direita é 1 nível mais alta que a esquerda). Isso também é considerado balanceado.
*   **FB = 2 (ou +2) ou FB = -2:** **DESBALANCEAMENTO!** A regra de ouro foi quebrada. A árvore precisa ser rebalanceada através de operações chamadas **rotações**.

### Exemplo Visual

```
      (10) FB=0       (10) FB=1        (10) FB=-1       (10) FB=2 -> DESBALANCEADO!
      /  \            /                \               /
    (5)  (15)        (5)                (15)           (5)
                    /                                  /
                   (3)                                (3)
```

## 3. A Estrutura do Nó (`Node.cs`)

Sua classe `Node` é o bloco de construção da árvore. Vamos analisar os campos:

```csharp
public class Node<T>
{
    public T Dado { get; set; }
    public Node<T>? Esquerda { get; set; }
    public Node<T>? Direita { get; set; }
    public Node<T>? Pai { get; set; } // Corrigido para aceitar nulo
    public int FatorBalanceamento { get; set; }

    public Node(T dado)
    {
        Dado = dado;
        Esquerda = null;
        Direita = null;
        Pai = null;
        FatorBalanceamento = 0; // Um novo nó (folha) sempre começa com FB = 0
    }
}
```

*   `Dado`: O valor armazenado no nó.
*   `Esquerda` e `Direita`: Ponteiros para os filhos.
*   `Pai`: Ponteiro para o nó pai. É **essencial** na sua implementação iterativa para poder subir na árvore e atualizar os fatores de balanceamento. O pai da raiz é `null`.
*   `FatorBalanceamento`: Como vimos, é a chave para manter a árvore balanceada.

Com esses conceitos em mente, podemos avançar para a **Inserção**.
