

class Program
{
    public static void Main()
    {
        Console.Clear();

        AVL<int> arvore = new AVL<int>();

        // Inserções do exemplo
        arvore.Insert(10);
        arvore.Insert(5);
        arvore.Insert(15);
        arvore.Insert(2);
        arvore.Insert(8);
        arvore.Insert(22);

        Console.WriteLine("Árvore AVL:");
        arvore.Mostrar();
    }
}