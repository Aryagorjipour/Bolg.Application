using BlogCommentService.ML.Data;
using Grpc.Core;
using Microsoft.Extensions.ML;

namespace BlogCommentService.Services;

public class CommentService : BlogCommentService.CommentService.CommentServiceBase
{

    private readonly PredictionEnginePool<CommentData, CommentPrediction> _predictionEnginePool;

    public CommentService(PredictionEnginePool<CommentData, CommentPrediction> predictionEnginePool)
    {
        _predictionEnginePool = predictionEnginePool;
    }

    public override Task<CommentResponse> AnalyzeComment(CommentRequest request, ServerCallContext context)
    {
        var commentData = new CommentData { Comment = request.Comment };
        var prediction = _predictionEnginePool.Predict(commentData);

        return Task.FromResult(new CommentResponse
        {
            Sentiment = prediction.Prediction ? "Positive" : "Negative",
            Score = prediction.Probability
        });
    }

    //public override Task<CommentResponse> AnalyzeComment(CommentRequest request, ServerCallContext context)
    //{
    //    var mlContext = new MLContext();

    //    // Load the data
    //    var dataPath = "data/comments.csv";
    //    var data = mlContext.Data.LoadFromTextFile<CommentData>(dataPath, hasHeader: true, separatorChar: ',');

    //    // Split the data into train and test sets
    //    var splitData = mlContext.Data.TrainTestSplit(data, testFraction: 0.2);

    //    // Define the data preparation and model pipeline
    //    var pipeline = mlContext.Transforms.Text.FeaturizeText("Features", nameof(CommentData.Comment))
    //        .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
    //            labelColumnName: nameof(CommentData.Label),
    //            featureColumnName: "Features"));

    //    // Train the model
    //    var model = pipeline.Fit(splitData.TrainSet);

    //    // Evaluate the model
    //    var predictions = model.Transform(splitData.TestSet);
    //    var metrics = mlContext.BinaryClassification.Evaluate(predictions);

    //    Console.WriteLine($"Accuracy: {metrics.Accuracy}");
    //    Console.WriteLine($"AUC: {metrics.AreaUnderRocCurve}");
    //    Console.WriteLine($"F1 Score: {metrics.F1Score}");

    //    // Save the model
    //    mlContext.Model.Save(model, data.Schema, "model.zip");

    //    // Test with a single prediction
    //    var predictionEngine = mlContext.Model.CreatePredictionEngine<CommentData, CommentPrediction>(model);
    //    var testComment = new CommentData { Comment = "PERFECT!" };
    //    var result = predictionEngine.Predict(testComment);

    //    Console.WriteLine($"Comment: {testComment.Comment}");
    //    Console.WriteLine($"Prediction: {(result.Prediction ? "Positive" : "Negative")}, Probability: {result.Probability}");

    //    Console.ReadLine();

    //    return Task.FromResult(new CommentResponse
    //    {
    //        Sentiment = result.Prediction ? "Positive" : "Negative",
    //        Score = result.Score
    //    });
    //}


}