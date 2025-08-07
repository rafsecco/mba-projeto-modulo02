import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { Router } from '@angular/router';
import { Usuario } from '../models/usuario';
import { ContaService } from '../services/conta.service';
import { LocalStorageUtils } from '../../utils/localstorage';
import {
  DisplayMessage,
  GenericValidator,
  ValidationMessages,
} from '../../utils/generic-form-validations';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-cadastro',
  standalone: true,
  templateUrl: './cadastro.component.html',
  imports: [CommonModule, ReactiveFormsModule],
})
export class CadastroComponent implements OnInit {
  cadastroForm!: FormGroup;
  errors: string[] = [];
  localStorageUtils = new LocalStorageUtils();

  genericValidator: GenericValidator;
  displayMessage: DisplayMessage = {};

  validationMessages: ValidationMessages = {
    email: {
      required: 'Informe o e-mail',
      email: 'Email inválido',
    },
    password: {
      required: 'Informe a senha',
      minlength: 'A senha deve ter no mínimo 6 caracteres',
    },
  };

  constructor(
    private fb: FormBuilder,
    private contaService: ContaService,
    private router: Router
  ) {
    this.genericValidator = new GenericValidator(this.validationMessages);
  }

  ngOnInit(): void {
    this.cadastroForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });

    this.cadastroForm.valueChanges.subscribe(() => {
      this.displayMessage = this.genericValidator.processarMensagens(
        this.cadastroForm
      );
    });
  }

  adicionarConta() {
    if (this.cadastroForm.dirty && this.cadastroForm.valid) {
      const usuario: Usuario = { ...this.cadastroForm.value };

      this.contaService.registraUsuario(usuario).subscribe({
        next: () => {
          this.contaService.login(usuario).subscribe({
            next: (response: any) => {
              this.localStorageUtils.salvarTokenUsuario(response.token);

              this.contaService.getCliente().subscribe({
                next: (usuario) => {
                  this.localStorageUtils.salvarUsuario(usuario);
                  this.router.navigate(['/home']);
                },
                error: (err) => {
                  console.error('Erro ao buscar usuário:', err);
                  this.errors.push('Erro ao carregar os dados do usuário.');
                },
              });
            },
            error: (err) => {
              console.error('Erro ao logar após cadastro:', err);
              this.errors.push('Erro ao logar após o cadastro.');
            },
          });
        },
        error: (err) => {
          console.error('Erro ao registrar:', err);
          this.errors.push('Erro ao registrar usuário.');
        },
      });
    }
  }
}
