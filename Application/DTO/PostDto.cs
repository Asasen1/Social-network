namespace Application.DTO;

public record PostDto(string Header, string Text, IReadOnlyList<PhotoDto> Photos);
