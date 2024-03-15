using HPCTech2024SpringProjectBoilerPlate.Shared;
using Microsoft.AspNetCore.Components;

namespace HPCTech2024SpringProjectBoilerPlate.Client.Components;

public partial class MovieDetails
{
    [Parameter]
    public OMDBMovie Movie { get; set; }

}
