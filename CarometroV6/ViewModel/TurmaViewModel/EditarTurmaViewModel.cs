﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CarometroV6.ViewModel.TurmaViewModel
{
    public class EditarTurmaViewModel
    {
        [Display(Name = "ID")]
        [Required(ErrorMessage = "Digite o ID da Turma")]
        [Range(1, 9999999, ErrorMessage = "Campo somente números")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Digite o nome da turma")]
        [StringLength(80, ErrorMessage = "Nome com no máximo 80 caracteres")]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "Digite a sigla da turma")]
        [StringLength(50, ErrorMessage = "Nome com no máximo 50 caracteres")]
        [Display(Name = "Sigla")]
        public string? Sigla { get; set; }

        [Required(ErrorMessage = "Digite código turma do SGSET")]
        [StringLength(20, ErrorMessage = "Nome com no máximo 20 caracteres")]
        [Display(Name = "Turma no SGSET")]
        public string? TurmaSgset { get; set; }

        [Display(Name = "URL Classroom")]
        public string? Classroom { get; set; }

        [Required(ErrorMessage = "Data início da turma")]
        [Display(Name = "Início da Turma")]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "Data fechamento da turma")]
        [Display(Name = "Fechamento da Turma")]
        public DateTime DataFim { get; set; }

        [Display(Name = "Turma Ativa")]
        public bool Ativo { get; set; }

        [Required(ErrorMessage = "Selecione o Curso")]
        [Display(Name = "Curso")]
        public int CursoId { get; set; }
        public SelectList? Cursos { get; set; }
    }
}
