using Application.Features.Brands.Dtos;
using Application.Features.Brands.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Brands.Commands.CreateBrand
{
    public partial class CreateBulkBrandCommand : IRequest<List<CreatedBrandDto>>
    {
        public string Name { get; set; }
        public List<string> NameList { get; set; }

        public class CreateBulkBrandCommandHandler : IRequestHandler<CreateBulkBrandCommand, List<CreatedBrandDto>>
        {
            private readonly IBrandRepository _brandRepository;
            private readonly IMapper _mapper;
            private readonly BrandBusinessRules _brandBusinessRules;

            public CreateBulkBrandCommandHandler(IBrandRepository brandRepository, IMapper mapper, BrandBusinessRules brandBusinessRules)
            {
                _brandRepository = brandRepository;
                _mapper = mapper;
                _brandBusinessRules = brandBusinessRules;
            }

            public async Task<List<CreatedBrandDto>> Handle(CreateBulkBrandCommand request, CancellationToken cancellationToken)
            {
                await _brandBusinessRules.BrandNameCanNotBeDuplicatedWhenInserted(request.Name);
                

                //Brand mappedBrand = _mapper.Map<Brand>(request);
                //Brand createdBrand = await _brandRepository.AddAsync(mappedBrand);

                List<Brand> mappedListBrand = new List<Brand>();
                foreach (var item in request.NameList)
                {
                    Brand brand = new Brand();
                    brand.Name = item;
                    mappedListBrand.Add(brand);
                }


                List<Brand> createdListBrand = await _brandRepository.AddRangeAsync(mappedListBrand);
                //CreatedBrandDto createdBrandDto = _mapper.Map<CreatedBrandDto>(createdBrand);
                CreatedBrandDto createdBrandDto = new CreatedBrandDto();
                var result = new List<CreatedBrandDto>();
                foreach (var item in createdListBrand)
                {
                    createdBrandDto = new CreatedBrandDto();
                    createdBrandDto.Id = item.Id;
                    createdBrandDto.Name = item.Name;
                    result.Add(createdBrandDto);

                }
                return result;

            }
        }
    }
}
