using DataService.Controllers;
using DataService.Core.Settings;
using DataService.Models.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System.Collections;
using System.Xml.Linq;
using WDS.Authorization;
using WDS.Models.JperStat;
using WDS.Swagger;

namespace DataServiceTesting
{
    public class SwaggerTests
    {

        internal class ServiceCollection : IServiceCollection
        {
            public ServiceDescriptor this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public int Count => throw new NotImplementedException();

            public bool IsReadOnly => throw new NotImplementedException();

            public void Add(ServiceDescriptor item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(ServiceDescriptor item)
            {
                throw new NotImplementedException();
            }

            public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<ServiceDescriptor> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            public int IndexOf(ServiceDescriptor item)
            {
                throw new NotImplementedException();
            }

            public void Insert(int index, ServiceDescriptor item)
            {
                throw new NotImplementedException();
            }

            public bool Remove(ServiceDescriptor item)
            {
                throw new NotImplementedException();
            }

            public void RemoveAt(int index)
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }

        }
        internal class HostingEnvironment : IWebHostEnvironment
        {
            public string EnvironmentName { get; set; } = "Development";

            public string? ApplicationName { get; set; } = "WDS";

            public string? WebRootPath { get; set; } = "C:\\\\Source\\\\WDS\\\\Source\\\\Server\\\\";

            public IFileProvider? WebRootFileProvider { get; set; }

            public string? ContentRootPath { get; set; } = "C:\\Source\\WDS\\Source\\Server";

            public IFileProvider? ContentRootFileProvider { get; set; }
        }
        internal class Config : IConfiguration
        {
            public string this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public IEnumerable<IConfigurationSection> GetChildren()
            {
                throw new NotImplementedException();
            }

            public IChangeToken GetReloadToken()
            {
                throw new NotImplementedException();
            }

            public IConfigurationSection GetSection(string key)
            {
                throw new NotImplementedException();
            }
        }
        internal class ServiceColleection : IServiceCollection
        {
            public ServiceDescriptor this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public int Count => throw new NotImplementedException();

            public bool IsReadOnly => throw new NotImplementedException();

            public void Add(ServiceDescriptor item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(ServiceDescriptor item)
            {
                throw new NotImplementedException();
            }

            public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<ServiceDescriptor> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            public int IndexOf(ServiceDescriptor item)
            {
                throw new NotImplementedException();
            }

            public void Insert(int index, ServiceDescriptor item)
            {
                throw new NotImplementedException();
            }

            public bool Remove(ServiceDescriptor item)
            {
                throw new NotImplementedException();
            }

            public void RemoveAt(int index)
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
        internal class ApplicationBuilder : IApplicationBuilder
        {
            public IServiceProvider ApplicationServices { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public IFeatureCollection ServerFeatures => throw new NotImplementedException();

            public IDictionary<string, object?> Properties => throw new NotImplementedException();

            public RequestDelegate Build()
            {
                throw new NotImplementedException();
            }

            public IApplicationBuilder New()
            {
                throw new NotImplementedException();
            }

            public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
            {
                throw new NotImplementedException();
            }
        }


        Config config = new Config();
        ServiceCollection services = new ServiceCollection();
        HostingEnvironment hosting = new HostingEnvironment();
        ApplicationBuilder application = new ApplicationBuilder();

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestAddSwagger()
        {
            try
            {
                SwaggerExtension.AddSwagger(services);
                Assert.Pass();
            }
            catch
            {
                Assert.Fail();
            }
        }


        [Test]
        public void TestUseCustomSwagger()
        {
            try
            {
                SwaggerExtension.UseCustomSwagger(application);
                Assert.Pass();
            }
            catch
            {
                Assert.Fail();
            }
        }
    }
}