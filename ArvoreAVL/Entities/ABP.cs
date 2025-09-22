

public class ABP<T> : IABP<T> where T : IComparable<T>
{
    protected Node<T>? Raiz;
    protected int Tamanho { get; set; }

    /////////// CONSTRUTORES ///////////
    public ABP()
    {
        Raiz = null;
        Tamanho = 0;
    }
    public ABP(T dado)
    {
        Raiz = new Node<T>(dado);
        Tamanho = 1;
    }


    // Metodos Principais
    public List<Node<T>> Children(Node<T> no)
    {
        if (Raiz is null) return new List<Node<T>>();

        List<Node<T>> list = new List<Node<T>>(2);

        if (no.Esquerda is not null) { list.Add(no.Esquerda); }
        if (no.Direita is not null) { list.Add(no.Direita); }

        return list;
    }

    public int Depth(Node<T> no)
    {
        if (no == Root()) return 0;

        return 1 + Depth(no.Pai);
    }

    public int Height()
    {
        return (Raiz is null) ? 0 : ContarAltura(Root());
    }
    protected int ContarAltura(Node<T> no)
    {
        if (no is null) return 0;

        int alturaDireita = ContarAltura(no.Direita);
        int alturaEsquerda = ContarAltura(no.Esquerda);

        return 1 + Math.Max(alturaDireita, alturaEsquerda);
    }

    public virtual void Insert(T dado)
    {
        Raiz = InserirDadoABP(Raiz, dado, null);
    }
    protected Node<T>? InserirDadoABP(Node<T>? noAtual, T dado, Node<T>? pai)
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
            noAtual.Esquerda = InserirDadoABP(noAtual.Esquerda, dado, noAtual);
        }
        else if (comparador > 0)
        {
            noAtual.Direita = InserirDadoABP(noAtual.Direita, dado, noAtual);
        }


        return noAtual;
    }

    public bool IsEmpty() => Raiz is null;

    public bool IsExternal(Node<T> no)
    {
        return no.Esquerda == null && no.Direita == null;
    }

    public bool IsInternal(Node<T> no)
    {
        return no.Esquerda is not null || no.Direita is not null;
    }

    public bool IsRoot(Node<T> no)
    {
        return no == Raiz;
    }

    public Node<T>? Parent(Node<T> no)
    {
        return no.Pai;
    }

    public Node<T>? Remove(T dado)
    {
        Raiz = RemoverDado(Raiz, dado);
        return Raiz;
    }
    protected Node<T>? RemoverDado(Node<T>? noAtual, T dado)
    {
        if (noAtual == null) return null;

        int comparador = dado.CompareTo(noAtual.Dado);

        if (comparador < 0)
        {
            noAtual.Esquerda = RemoverDado(noAtual.Esquerda, dado);
        }
        else if (comparador > 0)
        {
            noAtual.Direita = RemoverDado(noAtual.Direita, dado);
        }
        else if (comparador == 0) // O no foi encontrado, e sera removido
        {
            // Caso 1: É um nó folha e sera simplesmente removido
            if (noAtual.Esquerda is null && noAtual.Direita is null)
            {
                return null;
            }

            // Caso 2: So tem filho direito
            if (noAtual.Esquerda is null && noAtual.Direita is not null)
            {
                noAtual.Direita.Pai = noAtual.Pai;
                return noAtual.Direita;
            }

            // Caso 2: So tem filho Esquerdo
            if (noAtual.Esquerda is not null && noAtual.Direita is null)
            {
                noAtual.Esquerda.Pai = noAtual.Pai;
                return noAtual.Esquerda;
            }

            // Caso 3: Tem os dois filhos
            else
            {
                Node<T> sucessor = Minimo(noAtual.Direita);
                noAtual.Dado = sucessor.Dado;
                noAtual.Direita = RemoverDado(noAtual.Direita, sucessor.Dado);
            }
        }
        return noAtual;

    }
    protected Node<T> Minimo(Node<T> no)
    {
        while (no.Esquerda is not null)
        {
            no = no.Esquerda;
        }

        return no;
    }

    public T Replace(Node<T> no, T dado)
    {
        if (no == null)
            throw new ArgumentNullException(nameof(no));

        if (dado == null)
            throw new ArgumentNullException(nameof(dado));

        T antigo = no.Dado;
        no.Dado = dado;
        return antigo;
    }

    public Node<T>? Root()
    {
        return Raiz;
    }

    public Node<T>? Search(T dado)
    {
        return BuscarDado(Root(), dado);
    }

    protected Node<T>? BuscarDado(Node<T> noAtual, T dado)
    {
        if (noAtual is null) return null;

        int comparador = dado.CompareTo(noAtual.Dado);

        if (comparador == 0)
        {
            return noAtual;
        }
        else if (comparador < 0)
        {
            return BuscarDado(noAtual.Esquerda, dado);
        }
        else
        {
            return BuscarDado(noAtual.Direita, dado);
        }
    }

    public int Size()
    {
        return Tamanho;
    }

}