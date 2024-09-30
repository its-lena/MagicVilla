using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public VillaController(IVillaService villaService, IMapper mapper)
        {
            _villaService = villaService;
            _mapper = mapper;
        }

        public async Task<IActionResult> IndexVilla()
        {
            List<VillaDTO> list = new();

            var apiresponse = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.sessionToken));
            if (apiresponse != null && apiresponse.IsSuccess) 
            { 
                list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(apiresponse.Result));
            }
            return View(list);
        }
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> CreateVilla()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateVilla(VillaCreateDTO createVilla)
        {
            if(ModelState.IsValid)
            {
                var apiresponse = await _villaService.CreateAsync<APIResponse>(createVilla, HttpContext.Session.GetString(SD.sessionToken));

                if (apiresponse != null && apiresponse.IsSuccess)
                {
                    TempData["success"] = "Villa created Successfully";
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            TempData["error"] = "Error encountered";
            return View(createVilla);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVilla(int villaId)
        {
            var apiresponse = await _villaService.GetAsync<APIResponse>(villaId, HttpContext.Session.GetString(SD.sessionToken));

            if(apiresponse != null && apiresponse.IsSuccess)
            {
                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(apiresponse.Result));
                return View(_mapper.Map<VillaUpdateDTO>(model));
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDTO villaUpdateDTO)
        {
            if (ModelState.IsValid)
            {
                var apiresponse = await _villaService.UpdateAsync<APIResponse>(villaUpdateDTO, HttpContext.Session.GetString(SD.sessionToken));
                
                if(apiresponse != null && apiresponse.IsSuccess)
                {
                    TempData["success"] = "Villa updated Successfully";
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            TempData["error"] = "Error encountered";
            return View(villaUpdateDTO);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVilla(int villaId)
        {
            var apiresponse = await _villaService.GetAsync<APIResponse>(villaId, HttpContext.Session.GetString(SD.sessionToken));

            if (apiresponse != null && apiresponse.IsSuccess)
            {
                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(apiresponse.Result));
                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVilla(VillaDTO model)
        {
            var apiresponse = await _villaService.DeleteAsync<APIResponse>(model.Id, HttpContext.Session.GetString(SD.sessionToken));
            
            if( apiresponse != null && apiresponse.IsSuccess)
            {
                TempData["success"] = "Villa deleted Successfully";
                return RedirectToAction(nameof(IndexVilla));
            }
            TempData["error"] = "Error encountered";
            return View(model);
        }
    }
}
