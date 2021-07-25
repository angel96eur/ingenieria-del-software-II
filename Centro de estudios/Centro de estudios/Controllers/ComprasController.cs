using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Centro_de_estudios.Data;
using Centro_de_estudios.Models;
using Centro_de_estudios.Models.CompraViewModels;
using Microsoft.AspNetCore.Authorization;
using Centro_de_estudios.Models.CompraViewModels;


namespace Centro_de_estudios.Controllers
{
    [Authorize(Roles = "Estudiante")]
    public class ComprasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComprasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Compras
        public async Task<IActionResult> Index()
        {

            return View(_context.Compra.Include(p => p.Estudiante).Where(p => p.Estudiante.UserName.Equals(User.Identity.Name)).OrderByDescending(p => p.FechaCompra).ToList());
        }

        // GET: Compras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = _context.Compra.Include(p => p.CompraMateriales).ThenInclude<Compra, CompraMaterial, Material>(p => p.Material).Include(p => p.Estudiante).Where(p => p.CompraID == id).ToList();
                
            if (compra.Count == 0)
            {
                return NotFound();
            }

            return View(compra.First());
        }

        // GET: Compras/Create
        public IActionResult Create(SelectedMaterialesForCompraViewModel selectedMateriales)
        {
            Material material;
            int id;

            CompraCreateViewModel compra = new CompraCreateViewModel();
            compra.CompraMateriales = new List<CompraMaterial>();
            if (selectedMateriales.IdsToAdd == null)
                ModelState.AddModelError("MaterialNoSeleccionado", $"Debes de seleccionar un material a comprar");
            else
                foreach (string ids in selectedMateriales.IdsToAdd)
                {
                    id = int.Parse(ids);
                    material = _context.Material.Include(m => m.TipoMaterial).FirstOrDefault<Material>(m => m.MaterialID.Equals(id));
                    compra.CompraMateriales.Add(new CompraMaterial() { Cantidad = 1, Material = material });
                }
            Estudiante Estudiante = _context.Users.OfType<Estudiante>().FirstOrDefault<Estudiante>(u => u.UserName.Equals(User.Identity.Name));
            compra.Nombre = Estudiante.Nombre;
            compra.PrimerApellido = Estudiante.PrimerApellido;
            compra.SegundoApellido = Estudiante.SegundoApellido;

            return View(compra);
        }

        // POST: Compras/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(CompraCreateViewModel compraViewModel, IList<CompraMaterial> CompraMateriales, CreditCard CreditCard, PayPal PayPal)
        {
            Material material;
            Estudiante estudiante;
            Compra compra = new Compra();
            compra.PrecioTotal = 0;
            compra.CompraMateriales = new List<CompraMaterial>();
            ModelState.Clear();
            foreach(CompraMaterial item in CompraMateriales)
            {
                material = await _context.Material.FirstOrDefaultAsync<Material>(m => m.MaterialID == item.Material.MaterialID);
                if(material.CantidadCompra < item.Cantidad)
                {
                    ModelState.AddModelError("", $"No hay suficiente material titulado {material.Titulo}, por favor selecciones menos o igual que {material.CantidadCompra}");
                    

                  compraViewModel.CompraMateriales = CompraMateriales;
                    return View(compraViewModel);
                }
                else
                {
                    if (item.Cantidad > 0)
                    {
                        material.CantidadCompra = material.CantidadCompra - item.Cantidad;
                        item.Material = material;
                        item.Compra = compra;
                        compra.PrecioTotal += item.Cantidad * material.PrecioCompra;
                        compra.CompraMateriales.Add(item);
                    }

                    if(item.Cantidad < 0)
                    {
                        ModelState.AddModelError("", $"Por favor seleccione al menos un material a comprar o cancele compra");
                        compraViewModel.CompraMateriales = CompraMateriales;
                        return View(compraViewModel);
                    }

                    if (item.Cantidad == 0)
                    {
                        ModelState.AddModelError("", $"Por favor seleccione al menos un material a comprar o cancele compra");
                        compraViewModel.CompraMateriales = CompraMateriales;
                        return View(compraViewModel);
                    }

                }
            }
            estudiante = await _context.Users.OfType<Estudiante>().FirstOrDefaultAsync<Estudiante>(u => u.UserName.Equals(User.Identity.Name));

            if(ModelState.ErrorCount > 0)
            {
                compraViewModel.Nombre = estudiante.Nombre;
                compraViewModel.PrimerApellido = estudiante.PrimerApellido;
                compraViewModel.SegundoApellido = estudiante.SegundoApellido;
                return View(compraViewModel);
            }

            if (compra.PrecioTotal == 0)
            {
                compraViewModel.Nombre = estudiante.Nombre;
                compraViewModel.PrimerApellido = estudiante.PrimerApellido;
                compraViewModel.SegundoApellido = estudiante.SegundoApellido;
                ModelState.AddModelError("", $"Por favor seleccione al menos un material a comprar o cancele compra");
                compraViewModel.CompraMateriales = CompraMateriales;
                return View(compraViewModel);
            }

            compra.Estudiante = estudiante;
            compra.FechaCompra = DateTime.Now;


            if (compraViewModel.MetodoPago == "PayPal")
                compra.MetodoDePago = compraViewModel.PayPal;
            else
                compra.MetodoDePago = compraViewModel.CreditCard;
            compra.DeireccionDeEnvio = compraViewModel.DireccionEnvio;

            _context.Add(compra);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = compra.CompraID });
        }

        // GET: Compras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compra.Include(p => p.CompraMateriales).ThenInclude<Compra, CompraMaterial, Material>(i => i.Material).Include(p => p.Estudiante).SingleOrDefaultAsync(m => m.CompraID == id);
            if (compra == null)
            {
                return NotFound();
            }
            ViewData["EstudianteID"] = new SelectList(_context.Estudiante, "Id", "Id", compra.EstudianteID);
            return View(compra);
        }

        // POST: Compras/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompraID,EstudianteID,FechaCompra")] Compra compra)
        {
            if (id != compra.CompraID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(compra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompraExists(compra.CompraID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["EstudianteID"] = new SelectList(_context.Set<Estudiante>(), "Id", "Id", compra.EstudianteID);
            return View(compra);
        }

        // GET: Compras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compra.SingleOrDefaultAsync(m => m.CompraID == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // POST: Compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var compra = await _context.Compra.Include(p => p.CompraMateriales).SingleOrDefaultAsync(m => m.CompraID == id);
            _context.Compra.Remove(compra);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool CompraExists(int id)
        {
            return _context.Compra.Any(e => e.CompraID == id);
        }

        // GET: Compras/SelectMaterialesForCompra
        public IActionResult SelectMaterialesForCompra(string materialTitulo, string tipomaterialSelected)
        {
            SelectMaterialesForPurchaseViewModel selectMateriales = new SelectMaterialesForPurchaseViewModel();
            selectMateriales.TipoMateriales = new SelectList(_context.TipoMaterial.Select(g => g.Nombre).ToList());
            selectMateriales.Materiales = _context.Material.Include(m => m.TipoMaterial).Where(m => m.CantidadCompra > 0);
            if(materialTitulo != null)
                selectMateriales.Materiales = selectMateriales.Materiales.Where(m => m.Titulo.Contains(materialTitulo));
            if (tipomaterialSelected != null)
                selectMateriales.Materiales = selectMateriales.Materiales.Where(m => m.TipoMaterial.Nombre.Contains(tipomaterialSelected));
            selectMateriales.Materiales.ToList();

            return View(selectMateriales);
            

        }

        // POST: Compras/SelectMaterialesForCompra
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectMaterialesForCompra(SelectedMaterialesForCompraViewModel selectedMateriales)
        {
            if (selectedMateriales.IdsToAdd != null)
            {
                return RedirectToAction("Create", selectedMateriales);
            }
            
            ModelState.AddModelError(string.Empty, "Debes de seleccionar al menos un material");
            SelectMaterialesForPurchaseViewModel selectMateriales = new SelectMaterialesForPurchaseViewModel();
            selectMateriales.TipoMateriales = new SelectList(_context.TipoMaterial.Select(g => g.Nombre).ToList());
            selectMateriales.Materiales = _context.Material.Include(m => m.TipoMaterial).Where(m => m.CantidadCompra > 0).ToList();

            return View(selectMateriales);
        }
    }
}
