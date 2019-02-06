import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'env-var',
  templateUrl: './env-var.component.html'
})
export class EnvVarDataComponent {
  public envVariables: EnvironmentSystemVariable[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<EnvironmentSystemVariable[]>(baseUrl + 'api/SampleData/EnvironmentSystemVariables').subscribe(result => {
      this.envVariables = result;
    }, error => console.error(error));
  }
}

interface EnvironmentSystemVariable {
  name: string;
  value: string;
}
