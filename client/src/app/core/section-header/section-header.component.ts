import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BreadcrumbDefinition, BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-section-header',
  templateUrl: './section-header.component.html',
  styleUrls: ['./section-header.component.scss']
})
export class SectionHeaderComponent implements OnInit {
  breadcrumbs$: Observable<Array<BreadcrumbDefinition>>;

  constructor(private breadCrumbService: BreadcrumbService) { }

  ngOnInit(): void {
    this.breadcrumbs$ = this.breadCrumbService.breadcrumbs$;
  }

}
