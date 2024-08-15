using Microsoft.ML.Data;

namespace BlogCommentService.ML.Data;

public class CommentPrediction
{
    [ColumnName("PredictedLabel")]
    public bool Prediction { get; set; }

    public float Probability { get; set; }
    public float Score { get; set; }
}