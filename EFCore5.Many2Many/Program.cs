using EFCore3.Many2Many.Context;
using EFCore3.Many2Many.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace EFCore3.Many2Many
{
class Program
{
        static void Main(string[] args)
        {

            Console.WriteLine("Entity Framework 5");

            var options = new DbContextOptionsBuilder<EF3Context>()
.UseInMemoryDatabase(databaseName: "Test")
            .Options;

            using (var context = new EF3Context(options))
            {
                var user = new User { UserName = "José da Silva" };

                context.User.Add(user); // Adiciona um novo usuário ao Contexto

                var group = new Group { Name = "Administradores" };

                // Relaciona o usuário ao Novo Grupo ***** Sem entidade de junção
                group.Users.Add(user);

                // Adiciona um novo grupo ao Contexto
                context.Group.Add(group);

                // Persiste na banco de dados (in memory)
                context.SaveChanges();

                // Obtém os valores persistidos
                var users = context.User;
                var groups = context.Group;

                // Apresenta os valores persistidos
                Console.WriteLine();
                Console.WriteLine("Cadastros");
                Console.WriteLine("---------------");

                foreach (var item in users)
                    Console.WriteLine("Nome do Usuário: " + item.UserName);

                foreach (var item in groups)
                    Console.WriteLine("Nome do Grupo: " + item.Name);

                Console.WriteLine();
                Console.WriteLine("Relacionamentos");
                Console.WriteLine("---------------");


                foreach (var itemUser in users)
                    foreach (var itemGroup in itemUser.Groups) // Obtém valores many to many sem entidade de relacionamento
                        Console.WriteLine(itemUser.UserName + " é membro do grupo: " + itemGroup.Name);

                foreach (var itemGroup in groups)
                    foreach (var itemUser in itemGroup.Users) // Obtém valores many to many sem entidade de relacionamento
                        Console.WriteLine(itemGroup.Name + " tem como membro o usuário: " + itemUser.UserName);
            }

            Console.ReadKey();

        }
    }
}