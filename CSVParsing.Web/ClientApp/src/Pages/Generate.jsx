import React, { useState } from "react";

const Generate = () => {

    const [amount, setAmount] = useState('');

    const onGenerateClick = async () => {
        window.location.href = `/api/csvparsing/generate?amount=${amount}`;
    }

    return (
        <div className="d-flex vh-100" style={{ marginTop: -70 }}>
            <div className="d-flex w-100 justify-content-center align-self-center">
                <div className="row">
                    <input onChange={e => setAmount(e.target.value)} type="text" className="form-control-lg" placeholder="Amount" />
                </div>
                <div className="row">
                    <div className="col-md-4 offset-md-2">
                        <button className="btn btn-primary btn-lg" onClick={onGenerateClick}>Generate</button>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default Generate;