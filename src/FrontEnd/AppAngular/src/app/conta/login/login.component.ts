import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ContaService } from '../services/conta.service';
import { Usuario } from '../models/usuario';
import { LocalStorageUtils } from '../../utils/localstorage';
import { DisplayMessage, GenericValidator, ValidationMessages } from '../../utils/generic-form-validations';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.component.html',
  imports: [CommonModule, ReactiveFormsModule]
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  errors: string[] = [];
  localStorageUtils = new LocalStorageUtils();

  genericValidator: GenericValidator;
  displayMessage: DisplayMessage = {};

  validationMessages: ValidationMessages = {
    email: {
      required: 'Informe o e-mail',
      email: 'E-mail inválido'
    },
    password: {
      required: 'Informe a senha',
      minlength: 'A senha deve ter no mínimo 6 caracteres'
    }
  };

  constructor(
    private fb: FormBuilder,
    private contaService: ContaService,
    private router: Router
  ) {
    this.genericValidator = new GenericValidator(this.validationMessages);
  }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });

    this.loginForm.valueChanges.subscribe(() => {
      this.displayMessage = this.genericValidator.processarMensagens(this.loginForm);
    });
  }

  login() {
    if (this.loginForm.dirty && this.loginForm.valid) {
      const usuario = Object.assign({}, this.loginForm.value);

      this.contaService.login(usuario).subscribe({
        next: (response) => {
          this.localStorageUtils.salvarTokenUsuario(response.token);

          this.contaService.getCliente().subscribe({
            next: (usuario) => {
              this.localStorageUtils.salvarUsuario(usuario);
              this.router.navigate(['/home']);
            },
            error: () => {
              this.errors.push('Erro ao carregar os dados do usuário.');
            }
          });
        },
        error: () => {
          this.errors.push('Usuário ou senha incorretos.');
        }
      });
    }
  }
}
