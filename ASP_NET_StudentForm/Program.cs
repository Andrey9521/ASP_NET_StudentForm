var student = new List<Student>();

var wishes = new List<string>
{
    "Бажаю успіхів у всіх починаннях!",
    "Нехай кожен день приносить радість!",
    "Здоров’я та щастя вам і вашим близьким!",
    "Нехай здійсняться всі ваші мрії!",
    "Бажаю натхнення та нових ідей!",
    "Нехай удача завжди буде поруч!",
    "Миру та гармонії у вашому житті!",
    "Бажаю тепла і затишку у домі!",
    "Нехай кожен день буде особливим!",
    "Бажаю впевненості та сил для нових звершень!",
    "Нехай вас оточують добрі люди!",
    "Бажаю радості у кожній миті!",
    "Нехай робота приносить задоволення!",
    "Бажаю світлих думок і позитиву!",
    "Нехай щастя буде вашим постійним супутником!"
};

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", async (HttpContext context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.SendFileAsync("views/index.html");
});

app.MapGet("/formStudent", async (HttpContext context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.SendFileAsync("views/formaStudent.html");
});

app.MapPost("/createStudent", async (HttpContext context) =>
{
    var form = await context.Request.ReadFormAsync();

    var name = form["Name"];
    var age = int.TryParse(form["Age"], out var result) ? result : 0;
    var group = form["Group"];
    var isGrade = form["IsGrade"].Contains("true") ? "так" : "ні";

    var newStudent = new Student(name!, age, group!, isGrade);
    student.Add(newStudent);

    context.Response.Redirect("/");
});

app.MapGet("/students", () =>
{
    return Results.Json(student);
});

app.MapPost("/", (HttpContext context) =>
{
    var random = new Random();
    var wish = wishes[random.Next(wishes.Count)];
    return Results.Json(new { message = wish });
});

app.MapGet("/exportCsv", () =>
{
    var csv = "Name,Age,Group,IsGrade\n" +
              string.Join("\n", student.Select(s =>
                  $"{s.Name},{s.Age},{s.Group},{s.IsGrade}"));

    return Results.Text(csv, "text/csv");
});
app.Run();

record class Student(string Name, int Age, string Group, string IsGrade);