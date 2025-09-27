

public class AVL<T> : ABP<T>, IAVL<T> where T : IComparable<T>
{
    public AVL() : base() { }
    public AVL(T dado) : base(dado) { }

    // ======================
    // Sobrescrevendo Insert
    // ======================
    public override void Insert(T dado)
    {
        Raiz = InserirDadoAVL(Raiz, dado, null);
    }

    private Node<T> InserirDadoAVL(Node<T>? noAtual, T dado, Node<T>? pai)
    {
        if (noAtual is null)
        {
            var novo = new Node<T>(dado);
            novo.Pai = pai;
            Tamanho++;

            AtualizaFatorBalanceamentoInserir(pai, novo);

            return novo;
        }

        int comparador = dado.CompareTo(noAtual.Dado);

        if (comparador < 0)
        {
            noAtual.Esquerda = InserirDadoAVL(noAtual.Esquerda, dado, noAtual);
        }
        if (comparador > 0)
        {
            noAtual.Direita = InserirDadoAVL(noAtual.Direita, dado, noAtual);
        }

        return noAtual;
    }

    // ======================
    // Sobrescrevendo Remove
    // ======================

    // ======================
    // Sobrescrevendo Search
    // ======================

    // ======================
    // Rotações AVL
    // ======================
    public Node<T> RotacaoDuplaEsquerdaDireita(Node<T> pai)
    {
        pai.Esquerda = RotacaoSimplesEsquerda(pai.Esquerda);
        return RotacaoSimplesDireita(pai);
    }
    public Node<T> RotacaoDuplaDireitaEsquerda(Node<T> pai)
    {
        pai.Direita = RotacaoSimplesDireita(pai.Direita);
        return RotacaoSimplesEsquerda(pai);
        /*
        pai.Direita = RotacaoSimplesDireita(pai.Direita!);
        pai.Direita.Pai = pai; // garante pai atualizado
        return RotacaoSimplesEsquerda(pai);
        */
    }
    public Node<T> RotacaoSimplesDireita(Node<T> pai)
    {
        var filho = pai.Esquerda!; // Nó que vai subir

        // Subárvore direita do filho vira esquerda do pai
        pai.Esquerda = filho.Direita;

        if (filho.Direita != null) filho.Direita.Pai = pai;

        // Pai vira filho direito do filho
        filho.Direita = pai;

        // Ajusta ponteiros de pais
        filho.Pai = pai.Pai;
        pai.Pai = filho;

        AtualizaLigacaoComPai(filho, pai);

        // Atualiza fatores de balanceamento
        pai.FatorBalanceamento = pai.FatorBalanceamento - 1 - Math.Max(filho.FatorBalanceamento, 0);
        filho.FatorBalanceamento = filho.FatorBalanceamento - 1 + Math.Min(pai.FatorBalanceamento, 0);

        return filho;
    }
    public Node<T> RotacaoSimplesEsquerda(Node<T> pai)
    {
        var filho = pai.Direita!; // o nó que vai subir

        // Passa a subárvore esquerda do filho para ser a direita do pai
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
        pai.FatorBalanceamento = pai.FatorBalanceamento + 1 - Math.Min(filho.FatorBalanceamento, 0);
        filho.FatorBalanceamento = filho.FatorBalanceamento + 1 + Math.Max(pai.FatorBalanceamento, 0);

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
            if (pai.Esquerda == filho)
            {
                pai.FatorBalanceamento++;
            }
            else if (pai.Direita == filho)
            {
                pai.FatorBalanceamento--;
            }

            // Checa desbalanceamento
            if (pai.FatorBalanceamento == -2)
            {
                if (pai.Direita.FatorBalanceamento <= 0)
                {
                    pai = RotacaoSimplesEsquerda(pai);
                }
                else
                {
                    pai = RotacaoDuplaDireitaEsquerda(pai);
                }
            }
            else if (pai.FatorBalanceamento == 2)
            {
                if (pai.Esquerda.FatorBalanceamento >= 0)
                {
                    pai = RotacaoSimplesDireita(pai);
                }
                else
                {
                    pai = RotacaoDuplaEsquerdaDireita(pai);
                }
            }

            if (pai.FatorBalanceamento == 0) return; // O nó esta balanceado

            filho = pai;
            pai = pai.Pai;
        }
    }
}