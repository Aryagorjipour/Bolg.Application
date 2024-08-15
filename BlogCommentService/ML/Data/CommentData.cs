using Microsoft.ML.Data;

namespace BlogCommentService.ML.Data;

public class CommentData
{
    [LoadColumn(0)]
    public string Comment { get; set; }

    [LoadColumn(1)]
    public bool Label { get; set; }
}
