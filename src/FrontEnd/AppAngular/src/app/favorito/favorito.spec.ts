import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FavoritoComponent } from './favorito';

describe('Favorito', () => {
  let component: FavoritoComponent;
  let fixture: ComponentFixture<FavoritoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FavoritoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FavoritoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
