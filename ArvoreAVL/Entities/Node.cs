

public class Node<T>
{
    public T Dado { get; set; }
    public Node<T>? Esquerda { get; set; }
    public Node<T>? Direita { get; set; }
    public Node<T>? Pai { get; set; }
    public int FatorBalanceamento { get; set; }

    public Node(T dado)
    {
        Dado = dado;
        Esquerda = null;
        Direita = null;
        Pai = null;
        FatorBalanceamento = 0;
    }
}