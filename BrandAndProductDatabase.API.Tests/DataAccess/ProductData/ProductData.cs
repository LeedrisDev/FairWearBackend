﻿using System.Net;
using BrandAndProductDatabase.API.Models.Dto;
using BrandAndProductDatabase.API.Utils;
using BrandAndProductDatabase.API.Utils.HttpClientWrapper;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;

namespace BrandAndProductDatabase.API.Tests.DataAccess.ProductData;

[TestClass]
public class ProductData
{
    private Mock<IHttpClientWrapper> _httpClientWrapperMock;
    private API.DataAccess.ProductData.ProductData _productData;

    [TestInitialize]
    public void Initialize()
    {
        _httpClientWrapperMock = new Mock<IHttpClientWrapper>();
        _productData = new API.DataAccess.ProductData.ProductData(_httpClientWrapperMock.Object);
    }

    [TestMethod]
    public async Task GetProductByUpc_ReturnsProductRetrieverDto()
    {
        // Arrange
        var expectedUpc = "123456789";
        var expectedProductRetrieverDto = new ProductRetrieverDto
        {
            UpcCode = expectedUpc,
            BrandName = "NorthFace",
            Name = "Etip Hardface Glove",
            Category = "Gloves",
            Ranges = new List<string>()
            {
                "Men", "Women"
            }
        };

        var expectedResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonConvert.SerializeObject(expectedProductRetrieverDto))
        };

        _httpClientWrapperMock
            .Setup(mock => mock.GetAsync($"{AppConstants.ProductDataRetrieverUrl}/{expectedUpc}"))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _productData.GetProductByUpc(expectedUpc);

        // Assert
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(expectedProductRetrieverDto);
    }

    [TestMethod]
    public async Task GetProductByUpc_ReturnsNotFoundForNonExistentProduct()
    {
        // Arrange
        var expectedUpc = "123456789";
        var expectedResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NotFound
        };

        _httpClientWrapperMock
            .Setup(mock => mock.GetAsync($"{AppConstants.ProductDataRetrieverUrl}/{expectedUpc}"))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _productData.GetProductByUpc(expectedUpc);

        // Assert
        result.Status.Should().Be(HttpStatusCode.NotFound);
        result.ErrorMessage.Should().Be($"Product with barcode {expectedUpc} not found.");
    }

    [TestMethod]
    public async Task GetProductByUpc_ReturnsServiceUnavailableForUnsuccessfulRequest()
    {
        // Arrange
        var expectedUpc = "123456789";
        var expectedResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.InternalServerError
        };

        _httpClientWrapperMock
            .Setup(mock => mock.GetAsync($"{AppConstants.ProductDataRetrieverUrl}/{expectedUpc}"))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _productData.GetProductByUpc(expectedUpc);

        // Assert
        result.Status.Should().Be(HttpStatusCode.ServiceUnavailable);
        result.ErrorMessage.Should().Be($"Barcode service is unavailable.");
    }
}