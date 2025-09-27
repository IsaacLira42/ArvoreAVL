

class Program
{
    public static void Main()
    {
        Console.Clear();

        AVL<int> arvore = new AVL<int>();

        // Inserções do exemplo
        arvore.Insert(14);
        arvore.Mostrar();

        Console.WriteLine("");
        Console.WriteLine("-------------------------------------");
        Console.WriteLine("");

        arvore.Insert(27);
        arvore.Mostrar();

        Console.WriteLine("");
        Console.WriteLine("-------------------------------------");
        Console.WriteLine("");

        arvore.Insert(8);
        arvore.Mostrar();

        Console.WriteLine("");
        Console.WriteLine("-------------------------------------");
        Console.WriteLine("");

        arvore.Insert(19);
        arvore.Mostrar();

        Console.WriteLine("");
        Console.WriteLine("-------------------------------------");
        Console.WriteLine("");

        arvore.Insert(23);
        arvore.Mostrar();

        Console.WriteLine("");
        Console.WriteLine("-------------------------------------");
        Console.WriteLine("");

        // Console.WriteLine("Árvore AVL:");
        // arvore.Mostrar();
    }
}