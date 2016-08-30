import { Injectable } from '@angular/core';

import 'rxjs/add/operator/toPromise';

import { Hero } from './hero';

var HEROES: Hero[] = [
  {id: 11, name: 'Mr. Nice'},
  {id: 12, name: 'Narco'},
  {id: 13, name: 'Bombasto'},
  {id: 14, name: 'Celeritas'},
  {id: 15, name: 'Magneta'},
  {id: 16, name: 'RubberMan'},
  {id: 17, name: 'Dynama'},
  {id: 18, name: 'Dr IQ'},
  {id: 19, name: 'Magma'},
  {id: 20, name: 'Tornado'}
];
var nextId = 21;

@Injectable()
export class HeroService {
  getHeroes(): Promise<Hero[]> {
    return Promise.resolve(HEROES);
  }
  
  getHero(id: number): Promise<Hero> {
    return this.getHeroes()
              .then(heroes => heroes.find(hero => hero.id === id));
  }

  save(hero: Hero): Promise<Hero>  {
    if (hero.id) {
      return this.put(hero);
    }
    return this.post(hero);
  }

  delete(hero: Hero): Promise<any> {
    HEROES = HEROES.filter(h => h.id !== hero.id);
    return Promise.resolve();
  }

  // Add new Hero
  private post(hero: Hero): Promise<Hero> {
    hero.id = nextId++;
    HEROES.push(hero);
    return Promise.resolve(hero);
  }

  // Update existing Hero
  private put(hero: Hero): Promise<Hero> {
    let currentHero = HEROES.find(h => h.id === hero.id);
    currentHero.name = hero.name;
    return Promise.resolve(currentHero);
  }

  private handleError(error: any): Promise<any> {
    console.error('An error occurred', error);
    return Promise.reject(error.message || error);
  }
}

  