

public interface IABP<T>
{
    // Métodos Genéricos
    int Size();
    int Height();
    bool IsEmpty();

    // Métodos de Acesso
    Node<T>? Root();
    Node<T>? Parent(Node<T> no);
    List<Node<T>> Children(Node<T> no);

    // Métodos de Consulta
    bool IsInternal(Node<T> no);
    bool IsExternal(Node<T> no);
    bool IsRoot(Node<T> no);
    int Depth(Node<T> no);

    // Métodos de Atualização
    T Replace(Node<T> no, T dado);

    // Métodos adicionais
    void Insert(T dado);
    Node<T>? Remove(T dado);
    Node<T>? Search(T dado);
}