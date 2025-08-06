import { FormGroup } from '@angular/forms';

export type ValidationMessages = {
  [controlKey: string]: {
    [validationKey: string]: string;
  };
};

export type DisplayMessage = {
  [controlKey: string]: string;
};

export class GenericValidator {
  constructor(private validationMessages: ValidationMessages) {}

  processarMensagens(container: FormGroup): DisplayMessage {
    let messages: DisplayMessage = {};

    for (let controlKey in container.controls) {
      if (container.controls.hasOwnProperty(controlKey)) {
        let c = container.controls[controlKey];

        if (c instanceof FormGroup) {
          let childMessages = this.processarMensagens(c);
          Object.assign(messages, childMessages);
        } else {
          if (this.validationMessages[controlKey]) {
            messages[controlKey] = '';
            if ((c.dirty || c.touched) && c.errors) {
              Object.keys(c.errors).map(messageKey => {
                if (this.validationMessages[controlKey][messageKey]) {
                  messages[controlKey] += this.validationMessages[controlKey][messageKey] + ' ';
                }
              });
            }
          }
        }
      }
    }

    return messages;
  }
}
