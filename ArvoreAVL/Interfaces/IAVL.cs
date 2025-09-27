

public interface IAVL<T> : IABP<T>
{
    // ROTAÇÕES
    // Rotaçoes Simples
    Node<T> RotacaoSimplesEsquerda(Node<T> no);
    Node<T> RotacaoSimplesDireita(Node<T> no);
    // Rotaçoes Duplas
    Node<T> RotacaoDuplaEsquerdaDireita(Node<T> pai);
    Node<T> RotacaoDuplaDireitaEsquerda(Node<T> pai);

    // 
    void Insert(T dado);
    Node<T>? Remove(T dado);
    Node<T>? Search(T dado);
}