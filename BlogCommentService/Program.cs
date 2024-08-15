using BlogCommentService.Services;

var builder = WebApplication.CreateBuilder(args);


var dataPath = "data/comments.csv";
var modelPath = "model.zip";

ModelTrainer.TrainAndSaveModel(dataPath, modelPath);

ModelTrainer.ConfigureServices(builder.Services, modelPath);
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<CommentService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client");

app.Run();