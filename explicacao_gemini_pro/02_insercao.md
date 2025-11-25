# Guia de Estudo: Inserção em Árvore AVL

A inserção em uma árvore AVL ocorre em duas fases:

1.  **Fase 1: Inserção Padrão de ABP:** Encontrar o local correto e inserir o novo nó, como em qualquer Árvore Binária de Pesquisa.
2.  **Fase 2: Atualização e Rebalanceamento:** Subir de volta do nó inserido até a raiz, atualizando os Fatores de Balanceamento (FB) e realizando rotações se um desbalanceamento for encontrado.

Sua implementação `Insert` faz isso de forma iterativa, o que é muito eficiente.

```csharp
// Em ArvoreAVL/Entities/AVL.CS
public override void Insert(T dado)
{
    // ... (código para encontrar a posição e inserir o nó - Fase 1)

    // A chamada abaixo inicia a Fase 2
    AtualizaFatorBalanceamentoInserir(pai, novo);
}
```

## Fase 1: Inserção como em uma ABP

Sua implementação primeiro desce na árvore para encontrar onde o novo nó deve ser inserido.

1.  Começa pela `Raiz`.
2.  Compara o `dado` a ser inserido com o `dado` do nó atual.
3.  Se o `dado` for menor, vai para a `Esquerda`. Se for maior, vai para a `Direita`.
4.  Repete o processo até encontrar um ponteiro `null`. É aí que o novo nó é inserido.
5.  O `pai` do novo nó é o último nó visitado antes do `null`.

## Fase 2: Atualização e Rebalanceamento (`AtualizaFatorBalanceamentoInserir`)

Esta é a parte crucial da AVL. Após inserir o `novo` nó, o método sobe na árvore a partir do `pai` dele.

```csharp
private void AtualizaFatorBalanceamentoInserir(Node<T>? pai, Node<T> filho)
{
    while (pai is not null)
    {
        // 1. Atualiza o FB do pai
        if (pai.Esquerda == filho) pai.FatorBalanceamento++;
        else if (pai.Direita == filho) pai.FatorBalanceamento--;

        // 2. Checa o estado do pai
        if (pai.FatorBalanceamento == 0) return; // A altura da subárvore não mudou. Balanceado. Fim.

        if (pai.FatorBalanceamento == 2 || pai.FatorBalanceamento == -2)
        {
            // 3. DESBALANCEAMENTO! Precisa rotacionar.
            // ... (lógica de rotação) ...
            return; // Após a rotação, a árvore está balanceada. Fim.
        }

        // 4. Continua subindo na árvore
        filho = pai;
        pai = pai.Pai;
    }
}
```

### O Processo de Atualização:

-   **Passo 1:** Se o `filho` foi inserido à esquerda do `pai`, o `pai.FatorBalanceamento` aumenta (+1). Se foi à direita, diminui (-1).
-   **Passo 2:** Se o `pai.FatorBalanceamento` se torna `0`, significa que a subárvore que era menor cresceu, igualando as alturas. A altura total da subárvore do `pai` não mudou, então não precisamos mais propagar a atualização. O processo para.
-   **Passo 3:** Se o `pai.FatorBalanceamento` se torna `2` ou `-2`, encontramos um desbalanceamento. É hora de rotacionar!
-   **Passo 4:** Se o `pai.FatorBalanceamento` se torna `1` ou `-1`, a subárvore do `pai` aumentou de altura. Precisamos continuar subindo para avisar os ancestrais sobre essa mudança.

## As Rotações (O Coração da AVL)

Existem 4 casos de desbalanceamento que precisamos tratar. Eles são resolvidos com 2 tipos de rotações simples e 2 tipos de rotações duplas.

---

### Caso 1: Rotação Simples à Direita (Desbalanceamento Esquerda-Esquerda)

-   **Quando ocorre?** Quando um nó `P` fica com `FB = 2` e seu filho esquerdo `F` tem `FB = 1`. Isso significa que a inserção ocorreu na subárvore **esquerda** do filho **esquerdo**.
-   **Solução:** `RotacaoSimplesDireita(P)`

**Exemplo Visual:** Inserir `10` na árvore abaixo.

```
      (30) FB=1           (30) FB=2 -> Desbalanceado!          (20) <- Nova Raiz
      /                   /                                  /  \
    (20) FB=0   ->      (20) FB=1        -> Rotacao ->      (10)  (30)
    /                   /
  (10) <- Inserido    (10)
```

**O que o código `RotacaoSimplesDireita` faz:**

1.  O filho `F` (20) sobe e se torna a nova raiz da subárvore.
2.  O pai `P` (30) desce e se torna o filho direito de `F`.
3.  A subárvore direita de `F` (se existir) se torna a subárvore esquerda de `P`.
4.  Os ponteiros de `Pai` são atualizados.
5.  Os fatores de balanceamento são recalculados.

---


### Caso 2: Rotação Simples à Esquerda (Desbalanceamento Direita-Direita)

-   **Quando ocorre?** Quando um nó `P` fica com `FB = -2` e seu filho direito `F` tem `FB = -1`. A inserção ocorreu na subárvore **direita** do filho **direito**.
-   **Solução:** `RotacaoSimplesEsquerda(P)`

**Exemplo Visual:** Inserir `40`.

```
      (20) FB=-1          (20) FB=-2 -> Desbalanceado!         (30) <- Nova Raiz
        \                   \                                /  \
        (30) FB=0   ->      (30) FB=-1       -> Rotacao ->   (20)  (40)
          \
          (40) <- Inserido    (40)
```

**O que o código `RotacaoSimplesEsquerda` faz:** É o espelho da rotação à direita.

---


### Caso 3: Rotação Dupla Esquerda-Direita (Desbalanceamento Esquerda-Direita)

-   **Quando ocorre?** Quando um nó `P` fica com `FB = 2` e seu filho esquerdo `F` tem `FB = -1`. A inserção ocorreu na subárvore **direita** do filho **esquerdo**.
-   **Solução:** `RotacaoDuplaEsquerdaDireita(P)`

**Exemplo Visual:** Inserir `25`.

```
      (30) FB=1           (30) FB=2 -> Desbalanceado!
      /
    (20) FB=0   ->      (20) FB=-1
      \
      (25) <- Inserido    (25)
```

**O que o código `RotacaoDuplaEsquerdaDireita` faz:**

É uma combinação de duas rotações simples:

1.  **Primeiro, uma Rotação à Esquerda no filho `F` (20):** `RotacaoSimplesEsquerda(F)`. Isso transforma o caso "Esquerda-Direita" em um caso "Esquerda-Esquerda".
    ```
          (30) FB=2
          /
        (25) FB=1  <- Agora é um caso Esquerda-Esquerda!
        /
      (20)
    ```
2.  **Segundo, uma Rotação à Direita no pai `P` (30):** `RotacaoSimplesDireita(P)`.
    ```
          (25) <- Nova Raiz
         /    \
       (20)    (30)
    ```

Sua implementação reflete isso perfeitamente:
`pai.Esquerda = RotacaoSimplesEsquerda(pai.Esquerda!);`
`return RotacaoSimplesDireita(pai);`

---


### Caso 4: Rotação Dupla Direita-Esquerda (Desbalanceamento Direita-Esquerda)

-   **Quando ocorre?** Quando um nó `P` fica com `FB = -2` e seu filho direito `F` tem `FB = 1`. A inserção ocorreu na subárvore **esquerda** do filho **direito**.
-   **Solução:** `RotacaoDuplaDireitaEsquerda(P)`

**Exemplo Visual:** Inserir `25`.

```
      (20) FB=-1          (20) FB=-2 -> Desbalanceado!
        \
        (30) FB=0   ->      (30) FB=1
        /
      (25) <- Inserido    (25)
```

**O que o código `RotacaoDuplaDireitaEsquerda` faz:** É o espelho do caso 3.

1.  Primeiro, uma Rotação à Direita no filho `F` (30).
2.  Segundo, uma Rotação à Esquerda no pai `P` (20).

---

**Conclusão da Inserção:** Após a inserção, o caminho de volta até a raiz garante que qualquer desbalanceamento criado seja imediatamente corrigido pela rotação apropriada. Como a rotação restaura a propriedade de altura da subárvore, apenas **uma** rotação (simples ou dupla) é necessária para corrigir o desbalanceamento após uma inserção.

```