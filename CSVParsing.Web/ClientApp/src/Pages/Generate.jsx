import axios from "axios";
import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

const Generate = () => {
    
    const [amount, setAmount] = useState('');
    const navigate = useNavigate();

    const onGenerateClick = async () => {
        await axios.get(`/api/fileupload/generate?amount=${amount}`);
        navigate('/');
    }

    return (
        <div className="container" style={{marginTop: 60}}>
         <div className="d-flex vh-100" style={{marginTop: -70}}>
            <div className="d-flex w-100 justify-content-center align-self-center">
                <div className="row">
                    <input value={amount} onChange={e => setAmount(e.target.value)}  type="number" min="0" className="form-control-lg" placeholder="Amount" />
                </div>
                <div className="row">
                    <div className="col-md-4 offset-md-2">
                        <button className="btn btn-primary btn-lg" onClick={onGenerateClick}>Generate</button>
                    </div>
                </div>
            </div>
         </div>
        </div>
    )
}

export default Generate;