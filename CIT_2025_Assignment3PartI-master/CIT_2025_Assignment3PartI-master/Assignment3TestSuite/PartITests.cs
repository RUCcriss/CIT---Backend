using System;
using Xunit;

namespace Assignment3TestSuite;


public class PartITests
{
    //////////////////////////////////////////////////////////
    /// 
    /// Testing UrlParser class
    /// 
    ////////////////////////////////////////////////////////// 

    [Fact]
    public void UrlParser_ValidUrlWithoutId_ShouldParseCorrectly()
    {
        // Arrange
        var urlParser = new UrlParser();
        var url = "/api/categories";
        // Act
        var result = urlParser.ParseUrl(url);
        // Assert
        Assert.True(result);
        Assert.False(urlParser.HasId);
        Assert.Equal("/api/categories", urlParser.Path);
    }

    [Fact]
    public void UrlParser_ValidUrlWithId_ShouldParseCorrectly()
    {
        // Arrange
        var urlParser = new UrlParser();
        var url = "/api/categories/5";
        // Act
        var result = urlParser.ParseUrl(url);
        // Assert
        Assert.True(result);
        Assert.True(urlParser.HasId);
        Assert.Equal("5", urlParser.Id);
        Assert.Equal("/api/categories", urlParser.Path);
    }

    //////////////////////////////////////////////////////////
    /// 
    /// Testing RequestValidator class
    /// 
    //////////////////////////////////////////////////////////

    [Fact]
    public void RequestValidator_NoMethod_ShouldReturnMissingMethod()
    {
        // Arrange
        var requestValidator = new RequestValidator();
        var request = new Request
        {
            Path = "/api/xxx",
            Date = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()
        };
        // Act
        var result = requestValidator.ValidateRequest(request);
        // Assert
        Assert.Contains("missing method", result.Status);
    }

    [Fact]
    public void RequestValidator_InvalidMethod_ShouldReturnIllegalMethod()
    {
        // Arrange
        var requestValidator = new RequestValidator();
        var request = new Request
        {
            Method = "fetch",
            Path = "/api/categories/1",
            Date = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()
        };
        // Act
        var result = requestValidator.ValidateRequest(request);
        // Assert
        Assert.Contains("illegal method", result.Status);
    }

    [Fact]
    public void RequestValidator_NoPath_ShouldReturnMissingPath()
    {
        // Arrange
        var requestValidator = new RequestValidator();
        var request = new Request
        {
            Method = "read",
            Path = "",
            Date = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()
        };
        // Act
        var result = requestValidator.ValidateRequest(request);
        // Assert
        Assert.Contains("missing path", result.Status);
    }

    [Fact]
    public void RequestValidator_NoDate_ShouldReturnMissingDate()
    {
        // Arrange
        var requestValidator = new RequestValidator();
        var request = new Request
        {
            Method = "read",
            Path = "/api/xxx",
        };
        // Act
        var result = requestValidator.ValidateRequest(request);
        // Assert
        Assert.Contains("missing date", result.Status);
    }

    [Fact]
    public void RequestValidator_InvalidDate_ShouldReturnIllegalDate()
    {
        // Arrange
        var requestValidator = new RequestValidator();
        var request = new Request
        {
            Method = "read",
            Path = "/api/xxx",
            Date = DateTime.Now.ToString()
        };
        // Act
        var result = requestValidator.ValidateRequest(request);
        // Assert
        Assert.Contains("illegal date", result.Status);
    }

    [Theory]
    [InlineData("create")]
    [InlineData("update")]
    [InlineData("echo")]
    public void RequestValidator_NoBody_ShouldReturnMissingBody(string method)
    {
        // Arrange
        var requestValidator = new RequestValidator();
        var request = new Request
        {
            Method = method,
            Path = "/api/xxx",
            Date = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()
        };
        // Act
        var result = requestValidator.ValidateRequest(request);
        // Assert
        Assert.Contains("missing body", result.Status);
    }

    [Theory]
    [InlineData("create", "{\"id\":1,\"name\":\"xxx\"}")]
    [InlineData("update", "{\"id\":1,\"name\":\"xxx\"}")]
    public void RequestValidator_JasonBody_ShouldReturnOk(string method, string body)
    {
        // Arrange
        var requestValidator = new RequestValidator();
        var request = new Request
        {
            Method = method,
            Path = "/api/xxx",
            Date = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
            Body = body
        };
        // Act
        var result = requestValidator.ValidateRequest(request);
        // Assert
        Assert.Equal("1 Ok", result.Status);
    }

    [Theory]
    [InlineData("create", "xxx")]
    [InlineData("update", "xxx")]
    public void RequestValidator_NoJasonBody_ShouldReturnIllegalBody(string method, string body)
    {
        // Arrange
        var requestValidator = new RequestValidator();
        var request = new Request
        {
            Method = method,
            Path = "/api/xxx",
            Date = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
            Body = body
        };
        // Act
        var result = requestValidator.ValidateRequest(request);
        // Assert
        Assert.Contains("illegal body", result.Status);
    }


    [Fact]
    public void RequestValidator_ValidGetRequest_ShouldReturnTrue()
    {
        // Arrange
        var requestValidator = new RequestValidator();
        var request = new Request
        {
            Method = "read",
            Path = "/api/categories/1",
            Date = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()
        };
        // Act
        var result = requestValidator.ValidateRequest(request);
        // Assert
        Assert.Equal("1 Ok", result.Status);
    }

    

    



    //////////////////////////////////////////////////////////
    /// 
    /// Testing CategoryService class
    /// 
    //////////////////////////////////////////////////////////

    [Fact]
    public void CategoryService_GetAllCategories_ShouldReturnAllCategories()
    {
        // Arrange
        var categoryService = new CategoryService();
        // Act
        var categories = categoryService.GetCategories();
        // Assert
        Assert.NotNull(categories);
        Assert.Equal(3, categories.Count);
    }

    [Fact]
    public void CategoryService_GetCategoryById_ShouldReturnCorrectCategory()
    {
        // Arrange
        var categoryService = new CategoryService();
        // Act
        var category = categoryService.GetCategory(2);
        // Assert
        Assert.NotNull(category);
        Assert.Equal(2, category.Id);
        Assert.Equal("Condiments", category.Name);
    }

    [Fact]
    public void CategoryService_GetCategoryById_NonExistent()
    {
        // Arrange
        var categoryService = new CategoryService();
        // Act
        var category = categoryService.GetCategory(-1);
        // Assert
        Assert.Null(category);
    }

    [Fact]
    public void CategoryService_UpdateCategory_ShouldUpdateSuccessfully()
    {
        // Arrange
        var categoryService = new CategoryService();
        // Act
        var result = categoryService.UpdateCategory(1, "UpdatedName");
        var updatedCategory = categoryService.GetCategory(1);
        // Assert
        Assert.True(result);
        Assert.NotNull(updatedCategory);
        Assert.Equal("UpdatedName", updatedCategory.Name);
    }

    [Fact]
    public void CategoryService_UpdateCategory_NonExistent()
    {
        // Arrange
        var categoryService = new CategoryService();
        // Act
        var result = categoryService.UpdateCategory(-1, "UpdatedName");
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CategoryService_DeleteCategory_ShouldDeleteSuccessfully()
    {
        // Arrange
        var categoryService = new CategoryService();
        // Act
        var result = categoryService.DeleteCategory(3);
        var deletedCategory = categoryService.GetCategory(3);
        // Assert
        Assert.True(result);
        Assert.Null(deletedCategory);
    }

    [Fact]
    public void CategoryService_DeleteCategory_NonExistent()
    {
        // Arrange
        var categoryService = new CategoryService();
        // Act
        var result = categoryService.DeleteCategory(-1);
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CategoryService_CreateCategory_ShouldCreateSuccessfully()
    {
        // Arrange
        var categoryService = new CategoryService();
        // Act
        var result = categoryService.CreateCategory(4, "NewCategory");
        var newCategory = categoryService.GetCategory(4);
        // Assert
        Assert.True(result);
        Assert.NotNull(newCategory);
        Assert.Equal(4, newCategory.Id);
        Assert.Equal("NewCategory", newCategory.Name);
    }

    [Fact]
    public void CategoryService_CreateCategory_DuplicateId()
    {
        // Arrange
        var categoryService = new CategoryService();
        // Act
        var result = categoryService.CreateCategory(1, "DuplicateCategory");
        // Assert
        Assert.False(result);
    }

}


