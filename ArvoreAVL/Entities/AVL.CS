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
                return; // ? Coleguinha não especificou, então so ignora
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

        if (comparador < 0) pai!.Esquerda = novo;
        else pai!.Direita = novo;

        Tamanho++;

        AtualizaFatorBalanceamentoInserir(pai, novo);
    }

    public override Node<T>? Remove(T dado)
    {
        var no = Raiz;

        // Busca para encontrar o no
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
            var sucessor = no.Direita;
            while (sucessor!.Esquerda is not null) sucessor = sucessor.Esquerda;
            no.Dado = sucessor.Dado;

            // !! Parei, ovo continuar segunda
            no = sucessor;
        }

        // nò agor tem no máximo 1 filho
        Node<T>? filhoUnico = no.Esquerda ?? no.Direita;

        var paiNo = no.Pai;
        bool noEraEsquerdo = paiNo is not null && paiNo.Esquerda == no;

        if (filhoUnico is not null) filhoUnico.Pai = paiNo;

        if (paiNo is null)
        {
            Raiz = filhoUnico; // se for o raiz
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

    private List<List<Node<T>?>> ObterNosPorNivel()
    {
        var niveis = new List<List<Node<T>?>>();
        if (Raiz == null) return niveis;

        var fila = new Queue<Node<T>?>();
        fila.Enqueue(Raiz);

        while (fila.Count > 0)
        {
            var nivelAtual = new List<Node<T>?>();
            int tamanhoNivel = fila.Count;
            bool temNoNaoNulo = false;

            for (int i = 0; i < tamanhoNivel; i++)
            {
                var no = fila.Dequeue();
                nivelAtual.Add(no);
                if (no != null)
                {
                    temNoNaoNulo = true;
                    fila.Enqueue(no.Esquerda);
                    fila.Enqueue(no.Direita);
                }
                else
                {
                    fila.Enqueue(null);
                    fila.Enqueue(null);
                }
            }

            if (temNoNaoNulo)
            {
                niveis.Add(nivelAtual);
            }
            else
            {
                break;
            }
        }
        return niveis;
    }
    public void Mostrar()
    {
        if (Raiz == null)
        {
            Console.WriteLine("Árvore vazia");
            return;
        }

        var niveis = ObterNosPorNivel();
        int larguraConsole = 80;

        foreach (var nivel in niveis)
        {
            int quantidadeSlots = nivel.Count;
            int larguraSlot = larguraConsole / quantidadeSlots;
            string linha = "";

            foreach (var no in nivel)
            {
                string textoNo = "";
                if (no != null)
                {
                    textoNo = $"{no.Dado}[{no.FatorBalanceamento}]";
                }

                int preenchimento = (larguraSlot - textoNo.Length) / 2;
                if (preenchimento < 0) preenchimento = 0;

                linha += new string(' ', preenchimento);
                linha += textoNo;
                linha += new string(' ', larguraSlot - preenchimento - textoNo.Length);
            }
            Console.WriteLine(linha);
            Console.WriteLine();
        }
    }

    private void AtualizaFatorBalanceamentoInserir(Node<T>? pai, Node<T> filho)
    {
        while (pai is not null)
        {
            if (pai.Esquerda == filho) pai.FatorBalanceamento++;
            else if (pai.Direita == filho) pai.FatorBalanceamento--;

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
        // -1 se a subarvore da esquerda diminuiu, +1 se a da direita diminuiu
        while (no is not null)
        {
            no.FatorBalanceamento += ajuste;

            // altura n mudou, parar
            if (no.FatorBalanceamento == 1 || no.FatorBalanceamento == -1) return;

            else if (no.FatorBalanceamento == 0)
            {
                // altura diminuiu e vai subindo
                if (no.Pai is null) return;

                if (no.Pai.Esquerda == no)
                {
                    ajuste = -1;
                }
                else
                {
                    ajuste = 1;
                }

                no = no.Pai;
                continue;
            }

            // !! não consegui implementar os outros casos
            // !! Caso em que o fator de balanceamento é -2 ou 2
        }
    }

    public Node<T> RotacaoDuplaEsquerdaDireita(Node<T> pai)
    {
        pai.Esquerda = RotacaoSimplesEsquerda(pai.Esquerda!);

        if (pai.Esquerda is not null)
        {
            pai.Esquerda.Pai = pai;
        }
        return RotacaoSimplesDireita(pai);
    }
    public Node<T> RotacaoDuplaDireitaEsquerda(Node<T> pai)
    {
        pai.Direita = RotacaoSimplesDireita(pai.Direita!);

        if (pai.Direita is not null)
        {
            pai.Direita.Pai = pai;
        }

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

        pai.Direita = filho.Esquerda;

        if (filho.Esquerda is not null) filho.Esquerda.Pai = pai;

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

        return filho;
    }
}