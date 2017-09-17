import { Component, Inject } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { DatePipe } from '@angular/common';
import {IMyDpOptions} from 'mydatepicker';

@Component({
    selector: 'home',
    templateUrl: './home.component.html',
	providers: [DatePipe]
})
export class HomeComponent {
	public date : any;
	public albums : Album[];
	public http : Http;
	public baseUrl : string;
	public myDatePickerOptions: IMyDpOptions = {
        dateFormat: 'dd.mm.yyyy',
    };
	public model: any = { date: { year: 2018, month: 10, day: 9 } };

	constructor(http : Http, @Inject('BASE_URL') baseUrl: string, private datePipe: DatePipe) {
		this.http = http;
		this.date = this.datePipe.transform(new Date(), 'dd.MM.yyyy.');
		this.baseUrl = baseUrl;
		this.getAlbums(this.date);
	}

	getAlbums(date : any) : void {
		this.http.get(this.baseUrl + "api/albums?date=" + date).subscribe(result => {
			this.albums = result.json() as Album[];
		}, error => console.error(error));;
	}
}

interface Album {
	id : number;
	artist: string;
	name : string;
	date : Date;
	imageLarge : string;
}
