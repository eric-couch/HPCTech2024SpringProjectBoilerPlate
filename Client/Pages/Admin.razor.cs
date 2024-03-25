using HPCTech2024SpringProjectBoilerPlate.Shared;
using HPCTech2024SpringProjectBoilerPlate.Shared.Wrapper;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Notifications;
using HPCTech2024SpringProjectBoilerPlate.Client.HttpRepository;


namespace HPCTech2024SpringProjectBoilerPlate.Client.Pages;

public partial class Admin
{
    [Inject]
    IUserHttpRepository UserRepo { get; set; }
    public List<UserEditDto> Users { get; set; } = new List<UserEditDto>();

    protected override async Task OnInitializedAsync()
    {
        DataResponse<List<UserEditDto>> response = await UserRepo.GetAllUsersAsync();
        if (response.Succeeded)
        {
            Users = response.Data;
        } 
        // add error handling and notifications and logging
    }

    public async Task ToggleAdminUserEventHandler(ChangeEventArgs args, string UserId)
    {
        bool res = await UserRepo.ToggleAdminUser(UserId);
        if (!res) 
        {
            // toast error
        }
    }
}
