import { Component, Inject } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { DatePipe } from '@angular/common';

@Component({
    selector: 'home',
    templateUrl: './home.component.html',
	providers: [DatePipe]
})
export class HomeComponent {
	public date : any;
	public selectedDate : any;
	public albums : Album[];
	public http : Http;
	public baseUrl : string;
	public loaded : boolean;

	constructor(http : Http, @Inject('BASE_URL') baseUrl: string, private datePipe: DatePipe) {
		this.http = http;
		this.date = this.datePipe.transform(new Date(), 'dd.MM.yyyy.');
		this.selectedDate = this.datePipe.transform(new Date(), 'yyyy-MM-dd');
		this.baseUrl = baseUrl;
		this.getAlbums(this.selectedDate);
	}

	getAlbums(date : any) : void {
		this.albums = [];
		this.loaded = false;
		this.http.get(this.baseUrl + "api/albums?date=" + date).subscribe(result => {
			this.albums = result.json() as Album[];
			this.loaded = true;
		}, error => console.error(error));;
	}

	change() : void {
		this.date = this.datePipe.transform(this.selectedDate, 'dd.MM.yyyy.');
		this.getAlbums(this.selectedDate);
	}
}

interface Album {
	id : number;
	artist: string;
	name : string;
	releaseDate : Date;
	imageExtraLarge : string;
}
