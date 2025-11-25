# Explicação: `atualizaFatorDeBalanceamento(Nó v)`

O método `atualizaFatorDeBalanceamento` é o coração da árvore AVL. Sua responsabilidade é percorrer o caminho dos ancestrais de um nó recém-inserido ou removido, ajustar seus fatores de balanceamento e, crucialmente, identificar e corrigir qualquer desbalanceamento por meio de rotações. Para que o método funcione em $O(\log n)$, ele deve subir na árvore a partir do local da modificação até a raiz, parando assim que as alterações de altura não se propagarem mais.

A assinatura `void atualizaFatorDeBalanceamento(Nó v)` pode ser um pouco ambígua. O nó `v` de partida **deve ser o pai do nó que foi fisicamente inserido ou removido da árvore**.

- **Após Inserção:** `v` é o pai da nova folha que foi adicionada.
- **Após Remoção:** `v` é o pai do nó que foi efetivamente removido. (Lembre-se que se o nó removido tinha dois filhos, o que é fisicamente removido é o seu sucessor, que é uma folha ou um nó com apenas um filho).

## Algoritmo Geral

O algoritmo consiste em um laço que começa em `v` e sobe em direção à raiz usando os ponteiros de `Pai`. Para unificar a lógica de inserção e remoção, podemos pensar em termos de uma "variação de altura" (`delta`), onde `delta = +1` para uma inserção (a sub-árvore cresceu) e `delta = -1` para uma remoção (a sub-árvore encolheu).

O método a ser chamado seria `atualizaFatorDeBalanceamento(paiDoNoModificado, noModificado, delta)`.

```csharp
// v: o pai do nó que foi fisicamente alterado
// filho: o nó que referencia a sub-árvore que mudou de altura
// delta: +1 para inserção (cresceu), -1 para remoção (encolheu)
void atualizaFatorDeBalanceamento(Nó v, Nó filho, int delta)
{
    while (v != null)
    {
        // 1. ATUALIZA FATOR DE 'v'
        if (filho == v.Direita)
        {
            v.FatorBalanceamento -= delta;
        }
        else // filho == v.Esquerda
        {
            v.FatorBalanceamento += delta;
        }

        // 2. VERIFICA CONDIÇÕES DE PROPAGAÇÃO E ROTAÇÃO

        // Se o fator de balanceamento se tornou 0...
        if (v.FatorBalanceamento == 0)
        {
            // Na INSERÇÃO (delta=+1), FB ir de -1 ou +1 para 0 significa que a altura da sub-árvore NÃO mudou. Paramos.
            if (delta == 1) break;
            // Na REMOÇÃO (delta=-1), FB ir de -1 ou +1 para 0 significa que a altura da sub-árvore DIMINUIU. Devemos continuar subindo.
        }
        // Se o fator de balanceamento se tornou +1 ou -1...
        else if (v.FatorBalanceamento == 1 || v.FatorBalanceamento == -1)
        {
            // Na INSERÇÃO (delta=+1), FB ir de 0 para +1/-1 significa que a altura AUMENTOU. Devemos continuar subindo.
            if (delta == -1) break;
            // Na REMOÇÃO (delta=-1), FB ir de 0 para +1/-1 significa que a altura NÃO mudou. Paramos.
        }
        // Se o fator de balanceamento se tornou +2 ou -2 (DESBALANCEADO!)
        else
        {
            // 'rebalancear' realiza as rotações e retorna a nova raiz da sub-árvore.
            Nó novaRaizSubarvore = rebalancear(v);

            // Na REMOÇÃO, se a altura da sub-árvore diminuiu após o rebalanceamento (FB da nova raiz == 0),
            // a mudança de altura deve ser propagada para cima.
            if (delta == -1 && novaRaizSubarvore.FatorBalanceamento == 0)
            {
                // Prepara para a próxima iteração, subindo a partir da nova raiz da sub-árvore
                filho = novaRaizSubarvore;
                v = novaRaizSubarvore.Pai;
                // 'delta' continua -1, pois a mudança de altura continua se propagando
                continue;
            }
            else
            {
                // Na INSERÇÃO, a altura após rebalancear é sempre a original, então paramos.
                // Na REMOÇÃO, se a altura não diminuiu (FB != 0), a propagação também para.
                break;
            }
        }

        // 3. SOBE NA ÁRVORE para a próxima iteração
        filho = v;
        v = v.Pai;
    }
}
```

## Detalhando as Regras de Propagação

A chave é entender como o `FatorBalanceamento` de um nó ancestral `P` se comporta e o que isso significa para a altura da sub-árvore de `P`.

### Após uma INSERÇÃO (`delta = +1`)

A altura de uma sub-árvore só pode aumentar ou permanecer a mesma.
- **Se `P.FatorBalanceamento` se torna 0:** A sub-árvore de `P` ficou mais balanceada. Sua altura total **não mudou**. A propagação **para**.
- **Se `P.FatorBalanceamento` se torna +1 ou -1:** A sub-árvore de `P` cresceu em altura. A mudança **deve ser propagada** para o pai de `P`.
- **Se `P.FatorBalanceamento` se torna +2 ou -2:** Desbalanceamento! Uma ou duas rotações são necessárias. Após a rotação, a altura da nova sub-árvore é a mesma que a sub-árvore original tinha ANTES da inserção. Portanto, a propagação **para**.

### Após uma REMOÇÃO (`delta = -1`)

A altura de uma sub-árvore só pode diminuir ou permanecer a mesma.
- **Se `P.FatorBalanceamento` se torna +1 ou -1:** A sub-árvore de `P` não diminuiu de altura (o lado que encolheu já era o menor). A árvore absorveu o impacto da remoção nesse nível. A propagação **para**.
- **Se `P.FatorBalanceamento` se torna 0:** A sub-árvore de `P` **diminuiu** de altura. A mudança **deve ser propagada** para o pai de `P`.
- **Se `P.FatorBalanceamento` se torna +2 ou -2:** Desbalanceamento! Uma ou duas rotações são necessárias. Após a rotação, a altura da nova sub-árvore pode ter diminuído ou não.
    - Se o `FatorBalanceamento` da nova raiz da sub-árvore for 0, sua altura diminuiu, e a propagação **deve continuar** para cima.
    - Se for diferente de 0, a altura se estabilizou e a propagação **para**.

**Nota:** A função `rebalancear(v)` faria a checagem (`v.FatorBalanceamento == 2`, `v.Esquerda.FatorBalanceamento == 1`, etc.) e chamaria as rotações apropriadas (`RotacaoSimplesDireita`, `RotacaoDuplaEsquerdaDireita`, etc.). O pseudocódigo acima foca na lógica de *quando* chamar o rebalanceamento e como *propagar* a atualização de forma eficiente, garantindo a complexidade $O(\log n)$.
