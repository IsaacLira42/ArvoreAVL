

public class AVL<T> : ABP<T>, IAVL<T> where T : IComparable<T>
{
    public AVL() : base() { }
    public AVL(T dado) : base(dado) { }

    // ======================
    // Sobrescrevendo Insert
    // ======================
    public override void Insert(T dado)
    {
        if (Raiz is null)
        {
            Raiz = new Node<T>(dado);
            Tamanho = 1;
            return;
        }

        var atual = Raiz;
        
        Node<T>? pai = null;

        int cmp = 0;

        while (atual is not null)
        {
            pai = atual;
            cmp = dado.CompareTo(atual.Dado);
            if (cmp == 0)
            {
                // Ignora duplicados
                return;
            }
            else if (cmp < 0)
            {
                atual = atual.Esquerda;
            }
            else
            {
                atual = atual.Direita;
            }
        }

        var novo = new Node<T>(dado);
        novo.Pai = pai!;
        if (cmp < 0)
            pai!.Esquerda = novo;
        else
            pai!.Direita = novo;
        Tamanho++;

        AtualizaFatorBalanceamentoInserir(pai, novo);
    }

    private Node<T> InserirDadoAVL(Node<T>? noAtual, T dado, Node<T>? pai)
    {
        if (noAtual is null)
        {
            var novo = new Node<T>(dado);
            novo.Pai = pai;
            Tamanho++;
            return novo;
        }

        int comparador = dado.CompareTo(noAtual.Dado);

        if (comparador < 0)
        {
            var antigo = noAtual.Esquerda;
            noAtual.Esquerda = InserirDadoAVL(noAtual.Esquerda, dado, noAtual);
            if (antigo is null && noAtual.Esquerda is not null)
            {
                AtualizaFatorBalanceamentoInserir(noAtual, noAtual.Esquerda);
            }
        }
        if (comparador > 0)
        {
            var antigo = noAtual.Direita;
            noAtual.Direita = InserirDadoAVL(noAtual.Direita, dado, noAtual);
            if (antigo is null && noAtual.Direita is not null)
            {
                AtualizaFatorBalanceamentoInserir(noAtual, noAtual.Direita);
            }
        }

        return noAtual;
    }

    // ======================
    // Rotações AVL
    // ======================
    public Node<T> RotacaoDuplaEsquerdaDireita(Node<T> pai)
    {
        pai.Esquerda = RotacaoSimplesEsquerda(pai.Esquerda!);
        if (pai.Esquerda is not null) pai.Esquerda.Pai = pai;
        return RotacaoSimplesDireita(pai);
    }
    public Node<T> RotacaoDuplaDireitaEsquerda(Node<T> pai)
    {
        pai.Direita = RotacaoSimplesDireita(pai.Direita!);
        if (pai.Direita is not null) pai.Direita.Pai = pai;
        return RotacaoSimplesEsquerda(pai);
    }
    public Node<T> RotacaoSimplesDireita(Node<T> pai)
    {
        var filho = pai.Esquerda!; // Nó que vai subir

        // a arrvore direita do filho vira esquerda do pai
        pai.Esquerda = filho.Direita;

        if (filho.Direita != null) filho.Direita.Pai = pai;

        // pai vira filho direito do filho
        filho.Direita = pai;

        // ajusta referencias dos pais
        filho.Pai = pai.Pai;
        pai.Pai = filho;

        AtualizaLigacaoComPai(filho, pai);

        // atualiza fatores de balanceamento
        int bfPaiAntes = pai.FatorBalanceamento;
        int bfFilhoAntes = filho.FatorBalanceamento;
        pai.FatorBalanceamento = bfPaiAntes - 1 - Math.Max(bfFilhoAntes, 0);
        filho.FatorBalanceamento = bfFilhoAntes - 1 + Math.Min(bfPaiAntes, 0);

        return filho;
    }
    public Node<T> RotacaoSimplesEsquerda(Node<T> pai)
    {
        var filho = pai.Direita!; // o nó que vai subir

        // Passa a subarvore esquerda do filho para ser a direita do pai
        pai.Direita = filho.Esquerda;

        if (filho.Esquerda is not null) filho.Esquerda.Pai = pai;

        // Faz o pai virar filho a esquerda do novo no
        filho.Esquerda = pai;

        // Ajusta referencias de pais
        filho.Pai = pai.Pai;
        var paiAntigo = pai;
        pai.Pai = filho;

        // Conecta com o avô
        AtualizaLigacaoComPai(filho, paiAntigo);

        // Atualiza fatores de balanceamentp
        int bfPaiAntes = pai.FatorBalanceamento;
        int bfFilhoAntes = filho.FatorBalanceamento;
        pai.FatorBalanceamento = bfPaiAntes + 1 - Math.Min(bfFilhoAntes, 0);
        filho.FatorBalanceamento = bfFilhoAntes + 1 + Math.Max(bfPaiAntes, 0);

        return filho; // novo topo da rvore
    }
    private void AtualizaLigacaoComPai(Node<T> novoPai, Node<T> paiAntigo)
    {
        if (novoPai.Pai is null)
        {
            Raiz = novoPai;
            return;
        }

        if (novoPai.Pai.Esquerda == paiAntigo)
            novoPai.Pai.Esquerda = novoPai;
        else
            novoPai.Pai.Direita = novoPai;
    }

    // ======================
    // Mostrar Arvore
    // ======================
    public void Mostrar()
    {
        if (Raiz == null)
        {
            Console.WriteLine("Árvore vazia");
            return;
        }
        MostrarRecursivo(Raiz, 0);
    }
    public void MostrarInOrder()
    {
        if (Raiz == null)
        {
            Console.WriteLine("Árvore vazia");
            return;
        }
        MostrarInOrderRec(Raiz);
        Console.WriteLine();
    }
    private void MostrarInOrderRec(Node<T> no)
    {
        if (no is null) return;
        if (no.Esquerda is not null) MostrarInOrderRec(no.Esquerda);
        Console.Write($"{no.Dado}[{no.FatorBalanceamento}] ");
        if (no.Direita is not null) MostrarInOrderRec(no.Direita);
    }
    private void MostrarRecursivo(Node<T> no, int nivel)
    {
        if (no != null)
        {
            // Primeiro imprime a direita (para ficar com a raiz no meio)
            MostrarRecursivo(no.Direita, nivel + 1);

            // Imprime o nó atual com indentação proporcional ao nível
            string indentacao = new string(' ', nivel * 12);
            Console.WriteLine($"{indentacao}{no.Dado}[{no.FatorBalanceamento}]");

            // Depois imprime a esquerda
            MostrarRecursivo(no.Esquerda, nivel + 1);
        }
    }
    public void MostrarFormatado()
    {
        if (Raiz == null)
        {
            Console.WriteLine("Árvore vazia");
            return;
        }
        MostrarFormatadoRecursivo(Raiz, 0, "");
    }
    private void MostrarFormatadoRecursivo(Node<T> no, int nivel, string prefixo)
    {
        if (no != null)
        {
            if (nivel > 0)
            {
                Console.Write(prefixo);
                if (nivel > 1)
                {
                    Console.Write(new string(' ', (nivel - 1) * 12));
                }
            }

            Console.WriteLine($"{no.Dado}[{no.FatorBalanceamento}]");

            // Imprime filhos com indentação
            string novoPrefixo = nivel == 0 ? "" : prefixo + new string(' ', 12);
            MostrarFormatadoRecursivo(no.Direita, nivel + 1, novoPrefixo);
            MostrarFormatadoRecursivo(no.Esquerda, nivel + 1, novoPrefixo);
        }
    }

    // ======================
    // Auxiliares AVL
    // ======================
    private void AtualizaFatorBalanceamentoInserir(Node<T>? pai, Node<T> filho)
    {
        while (pai is not null)
        {
            // Agora é seguro usar ponteiros, pois a chamada ocorre após o link do filho no pai imediato
            if (pai.Esquerda == filho)
                pai.FatorBalanceamento++;
            else if (pai.Direita == filho)
                pai.FatorBalanceamento--;

            // Checa desbalanceamento
            if (pai.FatorBalanceamento == -2)
            {
                if (pai.Direita!.FatorBalanceamento <= 0)
                {
                    RotacaoSimplesEsquerda(pai);
                }
                else
                {
                    RotacaoDuplaDireitaEsquerda(pai);
                }
                // Após uma rotação durante inserção, a altura do subproblema é restaurada. Pode parar.
                return;
            }
            else if (pai.FatorBalanceamento == 2)
            {
                if (pai.Esquerda!.FatorBalanceamento >= 0)
                {
                    RotacaoSimplesDireita(pai);
                }
                else
                {
                    RotacaoDuplaEsquerdaDireita(pai);
                }
                // Após uma rotação durante inserção, a altura do subproblema é restaurada. Pode parar.
                return;
            }

            if (pai.FatorBalanceamento == 0) return; // O nó esta balanceado

            filho = pai;
            pai = pai.Pai;
        }
    }

    // ======================
    // Remoção AVL (O(log n))
    // ======================
    public override Node<T>? Remove(T dado)
    {
        // Primeiro, busca o nó e remove como ABP, mas precisamos saber o ponto onde a altura pode ter diminuído
        var no = Raiz;
        Node<T>? paiUltimo = null;
        Node<T>? ultimoVisitado = null;

        // Busca para encontrar o nó e também manter trilha do pai
        while (no is not null)
        {
            int cmp = dado.CompareTo(no.Dado);
            if (cmp == 0) break;
            ultimoVisitado = no;
            if (cmp < 0) no = no.Esquerda; else no = no.Direita;
        }

        if (no is null) return Raiz; // não existe

        // Caso com dois filhos: substitui pelo sucessor e remove o sucessor (como em ABP)
        if (no.Esquerda is not null && no.Direita is not null)
        {
            // encontra sucessor
            var s = no.Direita;
            while (s!.Esquerda is not null) s = s.Esquerda;
            no.Dado = s.Dado;
            // agora vamos remover s, que tem no máximo um filho à direita
            no = s;
        }

        // Agora no tem no máximo um filho
        Node<T>? filhoUnico = no.Esquerda ?? no.Direita;
        var paiNo = no.Pai;
        bool noEraEsquerdo = paiNo is not null && paiNo.Esquerda == no;

        if (filhoUnico is not null) filhoUnico.Pai = paiNo;

        if (paiNo is null)
        {
            Raiz = filhoUnico; // removendo a raiz
        }
        else if (noEraEsquerdo)
        {
            paiNo.Esquerda = filhoUnico;
        }
        else
        {
            paiNo.Direita = filhoUnico;
        }

        Tamanho--;

        // Rebalancear a partir do pai do nó removido
        if (paiNo is not null)
            RebalancearAposRemocao(paiNo, noEraEsquerdo ? -1 : +1);

        return Raiz;
    }
    private void RebalancearAposRemocao(Node<T> no, int delta)
    {
        // delta: -1 se a subárvore esquerda diminuiu, +1 se a subárvore direita diminuiu
        while (no is not null)
        {
            no.FatorBalanceamento += delta;

            if (no.FatorBalanceamento == 1 || no.FatorBalanceamento == -1)
            {
                // altura deste nó não mudou; parar
                return;
            }
            else if (no.FatorBalanceamento == 0)
            {
                // altura diminuiu; continuar subindo
                if (no.Pai is null) return;
                delta = (no.Pai.Esquerda == no) ? -1 : +1;
                no = no.Pai;
                continue;
            }
            else if (no.FatorBalanceamento == -2)
            {
                var r = no.Direita!;
                if (r.FatorBalanceamento <= 0)
                {
                    // Rotação simples à esquerda
                    RotacaoSimplesEsquerda(no);
                    if (r.FatorBalanceamento == 0)
                    {
                        // Após rotação com filho bf==0, a altura não diminui mais; parar
                        return;
                    }
                    else
                    {
                        // altura diminuiu; continuar com o pai do novo topo
                        var p = r.Pai; // após rotação, r sobe
                        if (p is null) return;
                        delta = (p.Pai != null && p.Pai.Esquerda == p) ? -1 : +1;
                        no = p.Pai!;
                        if (no is null) return;
                        continue;
                    }
                }
                else
                {
                    // Rotação dupla direita-esquerda
                    RotacaoDuplaDireitaEsquerda(no);
                    // Após rotação dupla, a altura diminui; continuar
                    var topo = no.Pai; // o novo topo da subárvore após rotação
                    if (topo is null) return;
                    delta = (topo.Pai != null && topo.Pai.Esquerda == topo) ? -1 : +1;
                    no = topo.Pai!;
                    if (no is null) return;
                    continue;
                }
            }
            else if (no.FatorBalanceamento == 2)
            {
                var l = no.Esquerda!;
                if (l.FatorBalanceamento >= 0)
                {
                    // Rotação simples à direita
                    RotacaoSimplesDireita(no);
                    if (l.FatorBalanceamento == 0)
                    {
                        // Após rotação com filho bf==0, parar
                        return;
                    }
                    else
                    {
                        var p = l.Pai; // após rotação, l sobe
                        if (p is null) return;
                        delta = (p.Pai != null && p.Pai.Esquerda == p) ? -1 : +1;
                        no = p.Pai!;
                        if (no is null) return;
                        continue;
                    }
                }
                else
                {
                    // Rotação dupla esquerda-direita
                    RotacaoDuplaEsquerdaDireita(no);
                    var topo = no.Pai;
                    if (topo is null) return;
                    delta = (topo.Pai != null && topo.Pai.Esquerda == topo) ? -1 : +1;
                    no = topo.Pai!;
                    if (no is null) return;
                    continue;
                }
            }
        }
    }
}