﻿using Application.Features.Languages.Dtos;
using Application.Features.Languages.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Languages.Commands.UpdateLanguage
{
    public class UpdateLanguageCommand : IRequest<UpdatedLanguageDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public class UpdateLanguageCommandHandler : IRequestHandler<UpdateLanguageCommand, UpdatedLanguageDto>
        {
            private readonly ILanguageRepository _languageRepository;
            private readonly IMapper _mapper;
            private readonly LanguageBusinessRules _languageBusinessRules;

            public UpdateLanguageCommandHandler(ILanguageRepository languageRepository, IMapper mapper, LanguageBusinessRules languageBusinessRules)
            {
                _languageRepository = languageRepository;
                _mapper = mapper;
                _languageBusinessRules = languageBusinessRules;
            }

            public async Task<UpdatedLanguageDto> Handle(UpdateLanguageCommand request, CancellationToken cancellationToken)
            {
                Language languageToUpdate = await _languageRepository.GetAsync(b => b.Id == request.Id);

                _languageBusinessRules.LanguageShouldExistWhenRequested(languageToUpdate);

                // Yeni adı kontrol et (mevcut isim değiştirilecekse)
                if (languageToUpdate.Name != request.Name)
                {
                    // Yeni adın benzersiz olup olmadığını kontrol et
                    await _languageBusinessRules.LanguageNameCanNotBeDuplicatedWhenInserted(request.Name);
                }

                languageToUpdate.Name = request.Name;

                Language updatedLanguage = await _languageRepository.UpdateAsync(languageToUpdate);

                UpdatedLanguageDto updatedLanguageDto = _mapper.Map<UpdatedLanguageDto>(updatedLanguage);

                return updatedLanguageDto;

            }
        }

    }
}
