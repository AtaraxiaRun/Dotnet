
using System.Xml.Linq;

namespace AspNetCore.Api.MiniApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = WebApplication.Create(args);

            #region ������ʹ��
            //���ã�curl http://localhost:5098/
            app.MapGet("/", () => "Hello World!");

            //���ã�curl http://localhost:5000/users/5/books/10
            app.MapGet("/users/{userId}/books/{bookId}",
    (int userId, int bookId) => $"The user id is {userId} and book id is {bookId}");
            #endregion

            #region ���׵�ʹ��Get,Post,Put,Delete
            var todos = new List<TodoItem>() { new TodoItem() { Id = 1, Name = "����" }, new TodoItem() { Id = 2, Name = "����2" } };

            //����: curl http://localhost:5098/todoitems
            app.MapGet("/todoitems", () => todos);

            //���ã�curl http://localhost:5098/todoitems/1/
            app.MapGet("/todoitems/{id}", (int id) =>
                todos.FirstOrDefault(t => t.Id == id)
                    is TodoItem todo
                        ? Results.Ok(todo)
                        : Results.NotFound());
            /*windows cmd���ã�curl -X POST http://localhost:5098/todoitems ^
     -H "Content-Type: application/json" ^
     -d "{\"Id\":3, \"Name\":\"����\", \"IsComplete\":false}"
            */
            app.MapPost("/todoitems", (TodoItem todo) =>
            {
                todos.Add(todo);    
                return Results.Created($"/todoitems/{todo.Id}", todo);
            });
            /*windows cmd���� ��curl -X PUT http://localhost:5098/todoitems/2 ^
     -H "Content-Type: application/json" ^
     -d "{\"Name\":\"����update\", \"IsComplete\":true}"

             */
            app.MapPut("/todoitems/{id}", (int id, TodoItem inputTodo) =>
            {
                var todo = todos.FirstOrDefault(t => t.Id == id);
                if (todo == null) return Results.NotFound();

                todo.Name = inputTodo.Name;
                todo.IsComplete = inputTodo.IsComplete;

                return Results.NoContent();
            });
            //���ã�curl -X DELETE http://localhost:5098/todoitems/1
            app.MapDelete("/todoitems/{id}", (int id) =>
            {
                var todo = todos.FirstOrDefault(t => t.Id == id);
                if (todo == null) return Results.NotFound();

                todos.Remove(todo);
                return Results.NoContent();
            });
            #endregion

            app.Run();
        }
    }

    public class TodoItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
}
