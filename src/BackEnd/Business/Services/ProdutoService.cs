using Business.Interfaces;
using Business.Models;
using Business.Utils;
using Business.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Security.Claims;

namespace Business.Services;

public class ProdutoService : IProdutoService
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly Guid _currentUserId;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProdutoRepository _produtoRepository;
    private readonly Upload _upload;

    public ProdutoService(IProdutoRepository produtoRepository, IHostEnvironment hostingEnvironment,
        IHttpContextAccessor httpContextAccessor, ICategoriaRepository categoriaRepository, Upload upload)
    {
        _produtoRepository = produtoRepository;
        _httpContextAccessor = httpContextAccessor;
        _categoriaRepository = categoriaRepository;
        _upload = upload;
        _currentUserId = GetCurrentUserId();
    }

    public async Task<List<Produto>> GetAsync(CancellationToken cancellationToken)
    {
        return await _produtoRepository.GetAsync(cancellationToken);
    }

    public async Task<List<Produto>> GetValidProductsAsync(CancellationToken cancellationToken)
    {
        return await _produtoRepository.GetValidProductsAsync(cancellationToken);
    }

    public async Task<List<Produto>> GetByVendedorId(CancellationToken cancellationToken)
    {
        return await _produtoRepository.GetByVendedorIdAsync(_currentUserId, cancellationToken);
    }

    public async Task<Produto> FindAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _produtoRepository.FindAsync(id, cancellationToken);
    }

    public async Task<List<Produto>> GetByCategoriaIdAsync(Guid categoriaId, CancellationToken cancellationToken)
    {
        return await _produtoRepository.GetByCategoriaIdAsync(categoriaId, cancellationToken);
    }

    public async Task<Guid> CreateAsync(CriaProdutoViewModel criaProdutoViewModel,
        CancellationToken cancellationToken, string? path = null)
    {
        var isValidCategory =
            await _categoriaRepository.IsValidCategoryAsync(criaProdutoViewModel.CategoriaId, cancellationToken);
        if (!isValidCategory)
            throw new KeyNotFoundException("Categoria inválida");

        var produto = new Produto
        {
            Nome = criaProdutoViewModel.Nome,
            Descricao = criaProdutoViewModel.Descricao,
            Preco = criaProdutoViewModel.Preco,
            Estoque = criaProdutoViewModel.Estoque,
            CategoriaId = criaProdutoViewModel.CategoriaId,
            VendedorId = _currentUserId
        };
        if (criaProdutoViewModel.ImagemUpload != null)
            await _upload.AddImageAsync(produto, criaProdutoViewModel.ImagemUpload, path, cancellationToken);

        return await _produtoRepository.CreateAsync(produto, cancellationToken);
    }

    public async Task UpdateAsync(AtualizaProdutoViewModel atualizaProdutoViewModel, CancellationToken cancellationToken, string? path = null)
    {
        var produto = await _produtoRepository.FindAsync(atualizaProdutoViewModel.Id, cancellationToken);
        if (produto is null) throw new KeyNotFoundException("Produto não encontrado");

        if (!IsUserOwner(produto))
            throw new UnauthorizedAccessException("Ação não permitida.");

        produto.Nome = atualizaProdutoViewModel.Nome ?? produto.Nome;
        produto.Descricao = atualizaProdutoViewModel.Descricao ?? produto.Descricao;
        produto.Preco = atualizaProdutoViewModel.Preco ?? produto.Preco;
        produto.Estoque = atualizaProdutoViewModel.Estoque ?? produto.Estoque;
        produto.Ativo = atualizaProdutoViewModel.Ativo;

		if (atualizaProdutoViewModel.ImagemUpload != null)
			await _upload.AddImageAsync(produto, atualizaProdutoViewModel.ImagemUpload, path, cancellationToken);

		await _produtoRepository.UpdateAsync(produto, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var produto = await _produtoRepository.FindAsync(id, cancellationToken);
        if (!IsUserOwner(produto))
            throw new UnauthorizedAccessException("Ação não permitida.");

        await _produtoRepository.DeleteAsync(id, cancellationToken);
    }

    private bool IsUserOwner(Produto? produto)
    {
        if (produto == null)
            return false;

        if (produto.VendedorId == _currentUserId)
            return true;

        // Verifica se tem a role de Admin
        var user = _httpContextAccessor.HttpContext?.User;
        var isAdmin = user?.IsInRole("Admin") ?? false;

        return isAdmin;
    }

    private Guid GetCurrentUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated != true)
            return Guid.Empty;

        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
            throw new Exception("Usuário não encontrado");

        return Guid.Parse(userId);
    }
}
