using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Models.VM;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;

        public VillaNumberController(IMapper mapper, IVillaNumberService villaNumberService, IVillaService villaService)
        {
            _mapper = mapper;
            _villaNumberService = villaNumberService;
            _villaService = villaService;
        }

        public async Task<IActionResult> IndexVillaNumber()
        {
            List<VillaNumberDTO> list = new();
            var apiresponse = await _villaNumberService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.sessionToken));

            if( apiresponse != null && apiresponse.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(apiresponse.Result));
            }
            return View(list);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateVillaNumber()
        {
            VillaNumberCreateVM vm = new VillaNumberCreateVM();
            var apiresponse = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.sessionToken));

            if(apiresponse != null && apiresponse.IsSuccess)
            {
                vm.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(apiresponse.Result)).Select(
                    x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    });
            }
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM villaNumberCreateVM)
        {
            if(ModelState.IsValid)
            {
                var apiresponse = await _villaNumberService.CreateAsync<APIResponse>(villaNumberCreateVM.VillaNumber, HttpContext.Session.GetString(SD.sessionToken));

                if(apiresponse != null && apiresponse.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    if(apiresponse.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", apiresponse.ErrorMessages.FirstOrDefault());
                    }
                }
            }

            var response = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.sessionToken));

            if(response != null && response.IsSuccess)
            {
                villaNumberCreateVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString((APIResponse)response.Result)).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
            }

            return View(villaNumberCreateVM);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVillaNumber(int villaNo)
        {
            VillaNumberUpdateVM villaNumberUpdateVM = new VillaNumberUpdateVM();
            var apiresponse = await _villaNumberService.GetAsync<APIResponse>(villaNo, HttpContext.Session.GetString(SD.sessionToken));

            if( apiresponse != null && apiresponse.IsSuccess)
            {
                VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(apiresponse.Result));
                villaNumberUpdateVM.VillaNumber = _mapper.Map<VillaNumberUpdateDTO>(model);
            }

            var response = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.sessionToken));

            if( response != null && response.IsSuccess)
            {
                villaNumberUpdateVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result)).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
            }

            return View(villaNumberUpdateVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM villaNumberUpdateVM)
        {
            var apiresponse = await _villaNumberService.UpdateAsync<APIResponse>(villaNumberUpdateVM.VillaNumber, HttpContext.Session.GetString(SD.sessionToken));

            if( apiresponse != null && apiresponse.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVillaNumber));
            }
            else
            {
                if(apiresponse.ErrorMessages.Count > 0)
                {
                    ModelState.AddModelError("ErrorMessage", apiresponse.ErrorMessages.FirstOrDefault());
                }
            }

            var response = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.sessionToken));
            villaNumberUpdateVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result)).Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            return View(villaNumberUpdateVM);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVillaNumber(int villaNo)
        {
            VillaNumberDeleteVM villaNumberDeleteVM = new();
            var apiresponse = await _villaNumberService.GetAsync<APIResponse>(villaNo, HttpContext.Session.GetString(SD.sessionToken));

            if (apiresponse != null && apiresponse.IsSuccess)
            {
                villaNumberDeleteVM.VillaNumber = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(apiresponse
                    .Result));
            }

            var response = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.sessionToken));

            if (response != null && response.IsSuccess)
            {
                villaNumberDeleteVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result)).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });

                return View(villaNumberDeleteVM);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVM model)
        {
            var apiresponse = await _villaNumberService.DeleteAsync<APIResponse>(model.VillaNumber.VillaNo, HttpContext.Session.GetString(SD.sessionToken));

            if (apiresponse != null && apiresponse.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVillaNumber));
            }
            return View(model);
        }

    }
}
