import { Component } from '@angular/core';
import { LoginService } from '../services/login.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  errorMessage: string | null = null;

  // Methoden vom service verwenden
  constructor(private loginservice: LoginService, private router: Router) {}

  // Hier den Benutzernamen überprüfen
  public login() {
    this.loginservice.easylogin(this.username, this.password).subscribe(
      (response) => {
        console.log('Login erfolgreich', response.message);
        // von hier aus zur TimeComponente weitergeleitet
        this.router.navigate(['/time']);
      },
      (error) => {
        this.errorMessage = 'Benutzername oder Passwort ist falsch';
        console.error('Login fehlgeschlagen', error);
      }
    );
  }
}
