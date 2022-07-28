import { Directive, Input, TemplateRef, ViewContainerRef } from "@angular/core";
import { AuthenticationService} from "../services/authenticationService";

@Directive({
    selector:"[rbacShow]"
})

export class RbacShowDirective  {
    allowedRoles:string[] = [];

    constructor(
        private templateRef: TemplateRef<any>,
        private viewContainer: ViewContainerRef,
        private authService: AuthenticationService) {
    }

    @Input()
    set rbacShow(allowedRoles: string[]) {
        this.allowedRoles = allowedRoles;
        console.log('allowed roles-', this.allowedRoles)
        if (!this.allowedRoles || this.allowedRoles.length === 0 ||
            !this.authService.currentUser) {
            this.viewContainer.clear();
            return;
        }

        const allowed:boolean = this.authService.currentUserValue.roles.filter
            (role => this.allowedRoles.includes(role)).length > 0;
        
        console.log('allowed-', this.authService.currentUserValue.roles)
        if (allowed) {
            this.viewContainer.createEmbeddedView(this.templateRef);
        }
        else {
            this.viewContainer.clear();
        }
    }
}