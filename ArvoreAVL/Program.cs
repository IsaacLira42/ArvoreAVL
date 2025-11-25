

class Program
{
    public static void Main()
    {
        Console.Clear();
        var arvore = new AVL<int>();
        arvore.Insert(50);
        arvore.Insert(80);
        arvore.Insert(30);
        arvore.Insert(65);
        arvore.Insert(100);
        arvore.Insert(130);
        arvore.Insert(62);
        arvore.Insert(20);
        arvore.Insert(5);
        arvore.Insert(7);
        arvore.Insert(3);
        arvore.Insert(25);
        arvore.Insert(1);
        arvore.Insert(2);


        while (true)
        {
            Console.WriteLine("\n==== Menu AVL ====");
            Console.WriteLine("1) Inserir");
            Console.WriteLine("2) Remover");
            Console.WriteLine("3) Buscar");
            Console.WriteLine("4) Mostrar (visual)");
            Console.WriteLine("5) Sair");
            Console.Write("Escolha: ");

            var opc = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(opc))
            {
                Console.WriteLine("Opção inválida.");
                continue;
            }

            if (opc == "5") break;

            try
            {
                switch (opc)
                {
                    case "1":
                        Console.Write("Valor inteiro para inserir: ");
                        if (int.TryParse(Console.ReadLine(), out int vi))
                        {
                            arvore.Insert(vi);
                            Console.WriteLine("Inserido.");
                        }
                        else
                        {
                            Console.WriteLine("Entrada inválida.");
                        }
                        break;
                    case "2":
                        Console.Write("Valor inteiro para remover: ");
                        if (int.TryParse(Console.ReadLine(), out int vr))
                        {
                            arvore.Remove(vr);
                            Console.WriteLine("Remoção solicitada.");
                        }
                        else
                        {
                            Console.WriteLine("Entrada inválida.");
                        }
                        break;
                    case "3":
                        Console.Write("Valor inteiro para buscar: ");
                        if (int.TryParse(Console.ReadLine(), out int vb))
                        {
                            var no = arvore.Search(vb);
                            if (no is not null)
                            {
                                Console.WriteLine($"Encontrado: {no.Dado} [FB={no.FatorBalanceamento}]");
                            }
                            else
                            {
                                Console.WriteLine("Não encontrado.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Entrada inválida.");
                        }
                        break;
                    case "4":
                        Console.WriteLine("\nÁrvore AVL (visual):");
                        arvore.Mostrar();
                        break;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }

        Console.WriteLine("Encerrado.");
    }
}