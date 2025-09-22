

public interface IAVL<T> : IABP<T>
{
    // ROTAÇÕES
    // Rotaçoes Simples
    void RotacaoSimplesEsquerda(Node<T> no);
    void RotacaoSimplesDireita(Node<T> no);
    // Rotaçoes Duplas
    void RotacaoDuplaEsquerda(Node<T> no);
    void RotacaoDuplaDireita(Node<T> no);

    // 
    void Insert(T dado);
    Node<T>? Remove(T dado);
    Node<T>? Search(T dado);
}