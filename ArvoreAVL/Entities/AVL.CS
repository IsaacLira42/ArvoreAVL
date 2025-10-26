public class AVL<T> : ABP<T>, IAVL<T> where T : IComparable<T>
{
    public AVL() : base() { }
    public AVL(T dado) : base(dado) { }

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
        int comparador = 0;

        while (atual is not null)
        {
            pai = atual;
            comparador = dado.CompareTo(atual.Dado);

            if (comparador == 0)
            {
                // ignora repetido
                return;
            }
            else if (comparador < 0)
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
        if (comparador < 0)
            pai!.Esquerda = novo;
        else
            pai!.Direita = novo;

        Tamanho++;

        AtualizaFatorBalanceamentoInserir(pai, novo);
    }

    // Metodo de inserir antigo (descartado)
    /*
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
    */

    public override Node<T>? Remove(T dado)
    {
        var no = Raiz;

        // Busca para encontrar o nó
        while (no is not null)
        {
            int comparador = dado.CompareTo(no.Dado);
            if (comparador == 0) break;
            if (comparador < 0) no = no.Esquerda; else no = no.Direita;
        }

        if (no is null) return Raiz; // valor nao encontrado entao sai

        // caso com 2 filhos, pega o sucessor
        if (no.Esquerda is not null && no.Direita is not null)
        {
            // acha o sucessor
            var s = no.Direita;
            while (s!.Esquerda is not null) s = s.Esquerda;
            no.Dado = s.Dado;
            // agora o problema é remover s
            no = s;
        }

        // no agr tem no max 1 filho
        Node<T>? filhoUnico = no.Esquerda ?? no.Direita;
        var paiNo = no.Pai;
        bool noEraEsquerdo = paiNo is not null && paiNo.Esquerda == no;

        if (filhoUnico is not null) filhoUnico.Pai = paiNo;

        if (paiNo is null)
        {
            Raiz = filhoUnico; // se for a raiz
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

        // Rebalanceia a arvore
        if (paiNo is not null) RebalancearAposRemocao(paiNo, noEraEsquerdo ? -1 : +1);

        return Raiz;
    }

    private List<List<Node<T>?>> GetNodesByLevel()
    {
        var levels = new List<List<Node<T>?>>();
        if (Raiz == null) return levels;

        var queue = new Queue<Node<T>?>();
        queue.Enqueue(Raiz);

        while (queue.Count > 0)
        {
            var currentLevel = new List<Node<T>?>();
            int levelSize = queue.Count;
            bool hasNonNullNode = false;

            for (int i = 0; i < levelSize; i++)
            {
                var node = queue.Dequeue();
                currentLevel.Add(node);
                if (node != null)
                {
                    hasNonNullNode = true;
                    queue.Enqueue(node.Esquerda);
                    queue.Enqueue(node.Direita);
                }
                else
                {
                    queue.Enqueue(null);
                    queue.Enqueue(null);
                }
            }

            if (hasNonNullNode)
            {
                levels.Add(currentLevel);
            }
            else
            {
                break;
            }
        }
        return levels;
    }
    public void Mostrar()
    {
        if (Raiz == null)
        {
            Console.WriteLine("Árvore vazia");
            return;
        }

        var levels = GetNodesByLevel();
        int consoleWidth = 80;

        foreach (var level in levels)
        {
            int slotCount = level.Count;
            int slotWidth = consoleWidth / slotCount;
            string line = "";

            foreach (var node in level)
            {
                string nodeText = "";
                if (node != null)
                {
                    nodeText = $"{node.Dado}[{node.FatorBalanceamento}]";
                }

                int padding = (slotWidth - nodeText.Length) / 2;
                if (padding < 0) padding = 0;

                line += new string(' ', padding);
                line += nodeText;
                line += new string(' ', slotWidth - padding - nodeText.Length);
            }
            Console.WriteLine(line);
        }
    }

    private void AtualizaFatorBalanceamentoInserir(Node<T>? pai, Node<T> filho)
    {
        while (pai is not null)
        {
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

                return;
            }

            if (pai.FatorBalanceamento == 0) return; // O nó esta balanceado

            filho = pai;
            pai = pai.Pai;
        }
    }
    private void RebalancearAposRemocao(Node<T> no, int ajuste)
    {
        // ajuste: -1 se a subarvore da esquerda diminuiu, +1 se a da direita diminuiu
        while (no is not null)
        {
            no.FatorBalanceamento += ajuste;

            // altura n mudou, parar
            if (no.FatorBalanceamento == 1 || no.FatorBalanceamento == -1) return;

            else if (no.FatorBalanceamento == 0)
            {
                // altura diminuiu, tem que continuar subindo
                if (no.Pai is null) return;

                if (no.Pai.Esquerda == no) {
                    ajuste = -1;
                } else {
                    ajuste = 1;
                }

                no = no.Pai;
                continue;
            }
            else if (no.FatorBalanceamento == -2)
            {
                var filhoDireita = no.Direita!;
                if (filhoDireita.FatorBalanceamento <= 0)
                {
                    // Rotacao simples a esquerda
                    RotacaoSimplesEsquerda(no);
                    if (filhoDireita.FatorBalanceamento == 0)
                    {
                        // caso especial, a altura nao diminui mais, parar
                        return;
                    }
                    else
                    {
                        // altura diminuiu, continuar com o pai do novo topo
                        var novoPai = filhoDireita.Pai;
                        if (novoPai is null) return;

                        if (novoPai.Pai != null && novoPai.Pai.Esquerda == novoPai) {
                            ajuste = -1;
                        } else {
                            ajuste = 1;
                        }

                        no = novoPai.Pai!;
                        if (no is null) return;
                        continue;
                    }
                }
                else
                {
                    // Rotacao dupla
                    RotacaoDuplaDireitaEsquerda(no);

                    var topo = no.Pai;
                    if (topo is null) return;

                    if (topo.Pai != null && topo.Pai.Esquerda == topo) {
                        ajuste = -1;
                    } else {
                        ajuste = 1;
                    }

                    no = topo.Pai!;
                    if (no is null) return;
                    continue;
                }
            }
            else if (no.FatorBalanceamento == 2)
            {
                var filhoEsquerda = no.Esquerda!;
                if (filhoEsquerda.FatorBalanceamento >= 0)
                {
                    // Rotacao simples a direita
                    RotacaoSimplesDireita(no);
                    if (filhoEsquerda.FatorBalanceamento == 0)
                    {
                        // caso especial, parar
                        return;
                    }
                    else
                    {
                        var novoPai = filhoEsquerda.Pai;
                        if (novoPai is null) return;

                        if (novoPai.Pai != null && novoPai.Pai.Esquerda == novoPai) {
                            ajuste = -1;
                        } else {
                            ajuste = 1;
                        }

                        no = novoPai.Pai!;
                        if (no is null) return;
                        continue;
                    }
                }
                else
                {
                    // Rotacao dupla
                    RotacaoDuplaEsquerdaDireita(no);
                    var topo = no.Pai;
                    if (topo is null) return;

                    if (topo.Pai != null && topo.Pai.Esquerda == topo) {
                        ajuste = -1;
                    } else {
                        ajuste = 1;
                    }

                    no = topo.Pai!;
                    if (no is null) return;
                    continue;
                }
            }
        }
    }

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
        var filho = pai.Esquerda!;

        // a arvore direita do filho vira esquerda do pai
        pai.Esquerda = filho.Direita;

        if (filho.Direita != null) filho.Direita.Pai = pai;

        // pai vira filho direito do filho
        filho.Direita = pai;

        // ajusta referencias dos pais
        filho.Pai = pai.Pai;
        pai.Pai = filho;

        // Atualiza a ligacao com o pai do nó que subiu
        if (filho.Pai is null)
        {
            Raiz = filho;
        }
        else
        {
            if (filho.Pai.Esquerda == pai)
                filho.Pai.Esquerda = filho;
            else
                filho.Pai.Direita = filho;
        }

        // atualiza fatores de balanceamento
        int fbPai = pai.FatorBalanceamento;
        int fbFilho = filho.FatorBalanceamento;
        pai.FatorBalanceamento = fbPai - 1 - Math.Max(fbFilho, 0);
        filho.FatorBalanceamento = fbFilho - 1 + Math.Min(fbPai, 0);

        return filho;
    }
    public Node<T> RotacaoSimplesEsquerda(Node<T> pai)
    {
        var filho = pai.Direita!;

        // Passa a subarvore esquerda do filho para ser a direita do pai
        pai.Direita = filho.Esquerda;

        if (filho.Esquerda is not null) filho.Esquerda.Pai = pai;

        // Faz o pai virar filho a esquerda do novo no
        filho.Esquerda = pai;

        // Ajusta referencias de pais
        filho.Pai = pai.Pai;
        pai.Pai = filho;

        // Conecta com o avô
        if (filho.Pai is null)
        {
            Raiz = filho;
        }
        else
        {
            if (filho.Pai.Esquerda == pai)
                filho.Pai.Esquerda = filho;
            else
                filho.Pai.Direita = filho;
        }

        // Atualiza fatores de balanceamentp
        int fbPai = pai.FatorBalanceamento;
        int fbFilho = filho.FatorBalanceamento;
        pai.FatorBalanceamento = fbPai + 1 - Math.Min(fbFilho, 0);
        filho.FatorBalanceamento = fbFilho + 1 + Math.Max(fbPai, 0);

        return filho; // novo topo
    }

}