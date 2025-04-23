using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels.Client;

namespace WebApp.Controllers;

[Authorize]
[Route("clients")]
public class ClientController(IClientService clientService) : Controller
{
    private readonly IClientService _clientService = clientService;

    [HttpGet]
    public async Task<IActionResult> ClientsList()
    {
        var model = new ClientViewModel
        {
            ClientList = await _clientService.GetAllClientsAsync(),
        };
        return View(model);
    }

    public IActionResult AddClient()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddClient(ClientRegistrationFormViewModel form)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray());
            return BadRequest(new { success = false, errors });
        }
        var result = await _clientService.CreateAsync(form);
        if (result != null)
            return RedirectToAction("ClientsList");
        return BadRequest(new
        {
            success = false,
            globalError = "Failed to create client"
        });
    }

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> UpdateClient(int id)
    {
        var client = await _clientService.GetClientByIdAsync(id);
        if (client == null)
            return NotFound();

        var viewModel = new ClientUpdateFormViewModel
        {
            Id = client.Id,
            ClientName = client.ClientName,
            Email = client.Email,

        };
        return PartialView("~/Views/Shared/Partials/Components/ClientsPartials/_UpdateClient.cshtml", viewModel);
    }

    [HttpPost("edit/{id}")]
    public async Task<IActionResult> UpdateClient(int id,ClientUpdateFormViewModel form)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray());
            return BadRequest(new { success = false, errors });
        }
        var result = await _clientService.UpdateAsync(id,form);
        if (result != null)
            return RedirectToAction("ClientsList");
        return BadRequest(new
        {
            success = false,
            globalError = "Failed to update client"
        });
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        var result = await _clientService.DeleteAsync(id);
        if (result)
            return RedirectToAction("ClientsList");

        return BadRequest();
    }
}
