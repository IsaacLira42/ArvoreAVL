# Guia de Estudo: Remoção em Árvore AVL

A remoção em uma árvore AVL é um pouco mais complexa que a inserção. Assim como a inserção, ela tem duas fases:

1.  **Fase 1: Remoção Padrão de ABP:** Encontrar e remover o nó, usando as regras de uma Árvore Binária de Pesquisa.
2.  **Fase 2: Atualização e Rebalanceamento:** Subir de volta do *pai do nó removido* até a raiz, atualizando os Fatores de Balanceamento (FB) e realizando rotações se um desbalanceamento for encontrado.

**Diferença Crucial da Inserção:** Na remoção, a altura de uma subárvore pode diminuir. Isso pode fazer com que **múltiplos desbalanceamentos** surjam ao longo do caminho até a raiz. Portanto, o processo de rebalanceamento não pode parar após a primeira rotação.

## Fase 1: Remoção como em uma ABP

Sua implementação `Remove` já inicia corretamente essa fase.

1.  **Nó com 0 ou 1 filho:** Este caso é simples. O nó é removido e, se tiver um filho, o filho toma o seu lugar.
    -   Sua implementação lida bem com isso: `Node<T>? filhoUnico = no.Esquerda ?? no.Direita;` e a lógica subsequente para conectar `filhoUnico` ao `paiNo`.

2.  **Nó com 2 filhos:** Este é o caso mais complexo.
    -   **Regra:** Para remover um nó com dois filhos, não o removemos diretamente. Em vez disso, encontramos o seu **sucessor** (o menor nó na subárvore direita), copiamos o dado do sucessor para o nó que queremos remover, e então removemos o nó sucessor.
    -   O nó sucessor, por definição, terá no máximo 1 filho (o filho direito), caindo no caso mais simples de remoção.
    -   Sua implementação identifica isso corretamente:
        ```csharp
        if (no.Esquerda is not null && no.Direita is not null)
        {
            // Acha o sucessor
            var sucessor = no.Direita;
            while (sucessor!.Esquerda is not null) sucessor = sucessor.Esquerda;
            no.Dado = sucessor.Dado; // Copia o dado

            no = sucessor; // !! Ponto de atenção: agora 'no' é o sucessor que será removido
        }
        ```

O ponto importante é que o rebalanceamento (Fase 2) deve começar a partir do **pai do nó que foi fisicamente removido**. No caso de 2 filhos, é o pai do *sucessor*.

## Fase 2: Rebalanceamento Pós-Remoção (`RebalancearAposRemocao`)

Esta é a parte que você precisa completar. O método deve subir na árvore a partir do `no` (o pai do nó fisicamente removido) e ajustar os fatores.

O loop `while (no is not null)` está correto. Dentro dele, a lógica precisa ser expandida.

### Lógica Completa para `RebalancearAposRemocao`

```csharp
private void RebalancearAposRemocao(Node<T> no, int ajuste)
{
    // 'ajuste' é +1 se um filho esquerdo foi removido (subárvore esquerda diminuiu)
    // 'ajuste' é -1 se um filho direito foi removido (subárvore direita diminuiu)

    while (no is not null)
    {
        no.FatorBalanceamento += ajuste;

        // Caso 1: A subárvore ficou mais balanceada.
        // A altura total da subárvore de 'no' não mudou. Fim da propagação.
        if (no.FatorBalanceamento == 1 || no.FatorBalanceamento == -1)
        {
            return;
        }

        // Caso 2: A subárvore ficou perfeitamente balanceada (FB foi de 1 ou -1 para 0).
        // A altura total da subárvore de 'no' DIMINUIU. A mudança deve ser propagada para cima.
        else if (no.FatorBalanceamento == 0)
        {
            // Prepara para a próxima iteração do loop
            if (no.Pai is null) return;
            ajuste = (no.Pai.Esquerda == no) ? 1 : -1;
            no = no.Pai;
            continue; // Continua subindo
        }

        // Caso 3: DESBALANCEAMENTO! (FB se tornou 2 ou -2). Hora de rotacionar.
        else // Fator de balanceamento é 2 ou -2
        {
            Node<T> pai = no.Pai; // Salva o pai antes da rotação
            bool eraFilhoEsquerdo = pai is not null && pai.Esquerda == no;

            // --- INÍCIO DA LÓGICA DE ROTAÇÃO ---

            if (no.FatorBalanceamento == 2) // Pende para a esquerda
            {
                // Rotação Simples à Direita (Caso Esquerda-Esquerda)
                if (no.Esquerda!.FatorBalanceamento >= 0)
                {
                    no = RotacaoSimplesDireita(no);
                }
                // Rotação Dupla Esquerda-Direita (Caso Esquerda-Direita)
                else
                {
                    no = RotacaoDuplaEsquerdaDireita(no);
                }
            }
            else // Fator de balanceamento é -2, pende para a direita
            {
                // Rotação Simples à Esquerda (Caso Direita-Direita)
                if (no.Direita!.FatorBalanceamento <= 0)
                {
                    no = RotacaoSimplesEsquerda(no);
                }
                // Rotação Dupla Direita-Esquerda (Caso Direita-Esquerda)
                else
                {
                    no = RotacaoDuplaDireitaEsquerda(no);
                }
            }

            // --- FIM DA LÓGICA DE ROTAÇÃO ---

            // Após a rotação, 'no' é a nova raiz da subárvore.
            // Conecta a subárvore rotacionada de volta ao pai.
            if (pai is not null)
            {
                if (eraFilhoEsquerdo) pai.Esquerda = no;
                else pai.Direita = no;
            }
            else
            {
                Raiz = no; // A raiz da árvore mudou
            }

            // IMPORTANTE: Se o FB da nova raiz ('no') se tornou 0 após a rotação,
            // a altura da subárvore diminuiu, e precisamos continuar subindo.
            if (no.FatorBalanceamento != 0)
            {
                return; // A altura se manteve, então podemos parar.
            }

            // Prepara para a próxima iteração
            if (pai is null) return;
            ajuste = (pai.Esquerda == no) ? 1 : -1;
            no = pai;
        }
    }
}
```

### Resumo dos Casos de Rotação na Remoção

A lógica é a mesma da inserção, mas o fator de balanceamento do filho é crucial.

-   **`P` com `FB = 2` (pesado à esquerda):**
    -   Se o filho esquerdo `F` tem `FB = 0` ou `FB = 1`: **Rotação Simples à Direita**.
    -   Se o filho esquerdo `F` tem `FB = -1`: **Rotação Dupla Esquerda-Direita**.

-   **`P` com `FB = -2` (pesado à direita):**
    -   Se o filho direito `F` tem `FB = 0` ou `FB = -1`: **Rotação Simples à Esquerda**.
    -   Se o filho direito `F` tem `FB = 1`: **Rotação Dupla Direita-Esquerda**.

O caso com `FB = 0` no filho é o que diferencia a remoção da inserção.

### Exemplo Visual (Rotação Simples na Remoção)

Imagine que removemos `D`, e `P` fica desbalanceado. O filho `F` tem `FB = 0`.

```
      (P) FB=1             (P) FB=2 -> Desbalanceado!
      /   \                /   \
    (F)FB=0 (D) -> Removido (F)FB=0
    /   \                  /   \
  (A)   (B)              (A)   (B)

      ||
      \/  RotacaoSimplesDireita(P)

      (F) FB=-1  <- A altura da subárvore diminuiu!
      /   \
    (A)   (P) FB=1
          /
        (B)
```

Note que após a rotação, a nova raiz `F` tem `FB = -1`. A altura total da subárvore diminuiu, então o processo de rebalanceamento **deve continuar** subindo a partir do pai de `F`. É por isso que o loop não pode parar sempre após a primeira rotação.

### Passos para o Sucesso na Prova:

1.  **Entenda os 4 casos de rotação:** Saiba identificar visualmente e pela combinação dos fatores de balanceamento (e.g., `P.FB=2`, `F.FB=1` -> Rotação Simples Direita).
2.  **Lembre-se da diferença entre Inserção e Remoção:**
    -   Inserção: Apenas uma rotação (simples ou dupla) é necessária. O processo para depois.
    -   Remoção: Podem ser necessárias múltiplas rotações. O processo continua subindo na árvore até a raiz ou até que a altura de uma subárvore se estabilize.
3.  **Pratique a implementação:** Tente reescrever a lógica de `RebalancearAposRemocao` no papel, seguindo o pseudocódigo/guia acima, para fixar os passos.

Boa sorte na prova!
