using BlogCommentService.ML.Data;
using Microsoft.Extensions.ML;
using Microsoft.ML;
using Microsoft.ML.Transforms.Text;

public static class ModelTrainer
{
    public static void TrainAndSaveModel(string dataPath, string modelPath)
    {
        var mlContext = new MLContext();

        var data = mlContext.Data.LoadFromTextFile<CommentData>(dataPath, hasHeader: true, separatorChar: ',');

        var pipeline = mlContext.Transforms.Text.TokenizeIntoWords("TokenizedComment", nameof(CommentData.Comment))
           .Append(mlContext.Transforms.Text.ApplyWordEmbedding("Features", "TokenizedComment", WordEmbeddingEstimator.PretrainedModelKind.GloVeTwitter25D))
           .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
               labelColumnName: nameof(CommentData.Label),
               featureColumnName: "Features"));

        var model = pipeline.Fit(data);

        mlContext.Model.Save(model, data.Schema, modelPath);
    }

    public static void ConfigureServices(IServiceCollection services, string modelPath)
    {
        services.AddPredictionEnginePool<CommentData, CommentPrediction>()
                .FromFile(modelPath);
    }
}
