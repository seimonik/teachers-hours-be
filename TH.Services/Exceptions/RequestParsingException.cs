namespace TH.Services.Exceptions;

public class RequestParsingException : Exception
{
	public int Row { get; set; }
	public int Column { get; set; }

	public RequestParsingException(string message, int row, int column) : base(message)
	{
		Row = row;
		Column = column;
	}
}
