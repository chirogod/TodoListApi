using TodoListApi.Migrations;

namespace TodoListApi.DTOs
{
    public class PaginationDTO<TodoItem>
    {
        public List<TodoItem> Data { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
        public int Total { get; set; }
    }
}
