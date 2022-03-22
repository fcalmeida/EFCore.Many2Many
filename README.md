# EFCore.Many2Many
Este é um exemplo de como criar um relacionamento many to many sem a necessidade de criar uma entidade de junção, e podendo mapear para uma tabela de banco de dados de forma explícita.

# Novidade Entity Framework Core 5
EF Core 5 trouxe um recurso bastante interessante e esperado por muitos programadores, e ao mesmo tempo, ainda não é muito conhecido. Se fizermos uma pesquisa sobre o assunto quase não vamos encontrar exemplos de como fazer um relacionamento many to many sem precisar usar uma entidade de junção usando fluent API para fazer o mapeamento para a tabela do banco de dados.
# Sobre o exemplo
O exemplo apresentado mostra como criar e fazer um relacionamento de muitos para muitos entre usuários e grupo de usuário, onde um usuário pode participar de mais de um grupo e os grupos podem ter mais de um usuário. Em outras palavras um clássico relacionamento de muitos para muitos.
# EFCore3.Many2Many
Esse projeto usa a versão 3 do EF Core, e implementa uma tabela de junção para fazer o relacionamento entre os usuários e grupos. O exemplo funciona nas versões mais recentes também, como a 5 e 6.

```
public class User
{
    public int UserID { get; set; }
    public string UserName { get; set; }

    public virtual ICollection<UserGroup> UserGroup { get; set; } = new List<UserGroup>();
}

public class Group
{
   public int GroupID { get; set; }
   public string Name { get; set; }

   public virtual ICollection<UserGroup> UserGroup { get; set; } = new List<UserGroup>();
}

public class UserGroup
{
   public int UserID { get; set; }
   public int GroupID { get; set; }

   public virtual Group Group { get; set; }
   public virtual User User { get; set; }
}
```

Observe que temos três entidades, uma que abstrai o usuário, outra o grupo de usuário e uma terceira que faz a junção de muitos para muitos. As entidades usuário e grupo contém uma coleção de usuários e grupos (UserGroup) que é um relacionamento com a entidade de junção.
Embora isso funcione, não é bem isso que esperamos de um ORM. O ideal é que ele de alguma forma faça a junção de forma implícita, sem a necessidade de criar uma entidade só para este fim.
Para definir o nome da tabela de junção (do banco de dados), as FKs e o nome delas, usando fluente API, podemos fazer da seguinte forma:

```
public class UserGroupConfig : IEntityTypeConfiguration<UserGroup>
{
    public void Configure(EntityTypeBuilder<UserGroup> builder)
    {
        builder.HasKey(t => new { t.UserID, t.GroupID }).HasName("pk_user_group");

        builder.ToTable("tb_user_group");

        builder.Property(t => t.UserID)
               .HasColumnName("UserID")
               .ValueGeneratedOnAdd()
               .IsRequired()
               .HasColumnType("int");

         builder.Property(t => t.GroupID)
                .HasColumnName("GroupID")
                .ValueGeneratedOnAdd()
                .IsRequired()
                .HasColumnType("int");

         builder.HasOne(t => t.User)
                .WithMany(t => t.UserGroup)
                .HasForeignKey(t => t.UserID)
                .HasConstraintName("fk_user_group_user");

         builder.HasOne(t => t.Group)
                .WithMany(t => t.UserGroup)
                .HasForeignKey(t => t.GroupID)
                .HasConstraintName("fk_group_group_user");
    }
}
```

Abaixo fazemos o mapeamento da entidade de junção para a tabela do banco de dados, e conseguimos definir os nomes.

```
         builder.HasOne(t => t.User)
                .WithMany(t => t.UserGroup)
                .HasForeignKey(t => t.UserID)
                .HasConstraintName("fk_user_group_user");

         builder.HasOne(t => t.Group)
                .WithMany(t => t.UserGroup)
                .HasForeignKey(t => t.GroupID)
                .HasConstraintName("fk_group_group_user");
```

No momento de adicionar um novo relacionamento, não é alguma coisa muito bonitinha, você precisa fazer o relacionamento criado uma nova instancia da entidade de junção.

```
  var user = new User { UserName = "José da Silva" };

  context.User.Add(user); // Adiciona um novo usuário ao Contexto

  var group = new Group { Name = "Administradores" };

  // Relaciona o usuário ao Novo Grupo ***** Com entidade de junção
  group.UserGroup.Add(new UserGroup { UserID = user.UserID });

  // Adiciona um novo grupo ao Contexto
  context.Group.Add(group);

  // Persiste na banco de dados (in memory)
  context.SaveChanges();
```

E para recuperar também é chatinho, veja:

```
    foreach (var itemUser in users)
        foreach (var itemGroup in itemUser.UserGroup) // Obtém valores many to many com entidade de relacionamento
            Console.WriteLine(itemUser.UserName + " é membro do grupo: " + itemGroup.Group.Name);

    foreach (var itemGroup in groups)
        foreach (var itemUser in itemGroup.UserGroup) // Obtém valores many to many com entidade de relacionamento
            Console.WriteLine(itemGroup.Name + " tem como membro o usuário: " + itemUser.User.UserName);
```

# EFCore5.Many2Many
Com o EF Core 5, as coisas ficam um pouco mais claras. Usaremos o mesmo exemplo, porém vamos fazer o mapeamento sem precisar criar uma entidade de junção.

```
public class User
{
    public int UserID { get; set; }
    public string UserName { get; set; }

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
 }

public class Group
{
    public int GroupID { get; set; }
    public string Name { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
```

Observe que agora não existe mais a entidade de junção e o relacionamento é direto entre usuário e grupo.
Neste ponto o mapeamento das entidades para o banco de dados fica bastante simples, e não precisamos de muitas declarações para conseguirmos um resultado melhor. 
Neste caso eu fiz o mapeamento na própria classe GroupConfig.

```
public void Configure(EntityTypeBuilder<Group> builder)
{
    builder.HasKey(t => t.GroupID).HasName("pk_tb_group");

    builder.ToTable("tb_group");

    builder.Property(t => t.GroupID)
           .HasColumnName("GroupID")
           .ValueGeneratedOnAdd()
           .IsRequired()
           .HasColumnType("int");

    builder.Property(t => t.Name)
           .HasColumnName("Name")
           .HasColumnType("varchar")
           .HasMaxLength(100)
           .IsRequired();

    builder.HasMany(t => t.Users)
           .WithMany(t => t.Groups)
           .UsingEntity<Dictionary<string, object>>("UserGroup",
              leftTable => leftTable.HasOne<User>()
                                    .WithMany()
                                    .HasForeignKey("UserID")
                                    .HasConstraintName("fk_user_group_user"),
              rightTable => rightTable.HasOne<Group>()
                                    .WithMany()
                                    .HasForeignKey("GroupID")
                                    .HasConstraintName("fk_group_group_user"),
               j =>
                      {
                          j.HasKey("UserID", "GroupID").HasName("pk_group_user");
                          j.ToTable("tb_group_user");
                      });
        }
```

No EF Core 5 podemos usar o UsingEntity para especificarmos uma junção, e isso é bem simples como o exemplo acima.

Para adicionar a informação ao contexto também é mais direto, claro e fácil, veja:

```
    var user = new User { UserName = "José da Silva" };

    context.User.Add(user); // Adiciona um novo usuário ao Contexto

    var group = new Group { Name = "Administradores" };

    // Relaciona o usuário ao Novo Grupo ***** Sem entidade de junção
    group.Users.Add(user);

    // Adiciona um novo grupo ao Contexto
    context.Group.Add(group);

    // Persiste na banco de dados (in memory)
    context.SaveChanges();

```

E a recuperação fica bem mais limpa e fácil.

```
    foreach (var itemUser in users)
        foreach (var itemGroup in itemUser.Groups) // Obtém valores many to many sem entidade de relacionamento
            Console.WriteLine(itemUser.UserName + " é membro do grupo: " + itemGroup.Name);

    foreach (var itemGroup in groups)
        foreach (var itemUser in itemGroup.Users) // Obtém valores many to many sem entidade de relacionamento
            Console.WriteLine(itemGroup.Name + " tem como membro o usuário: " + itemUser.UserName);
```

#Conclusão
O EF Core 5 é possível eliminar a tabela de junção, que e muito casos tornará a programação mais limpa e prática.
